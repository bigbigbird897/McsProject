using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Models.DBConfig;

using Microsoft.Extensions.Configuration;

using System.Collections.Concurrent;
using System.Reflection;

namespace CoreServiceLib.Core.DBCore
{
    class DbManager
    {
        private static readonly ConcurrentDictionary<string, DbConfigInfo> mDbBuffer = [];
        private static readonly IConfiguration config = McsApp.McsRegistry.GetConfiguration();
        private static readonly bool dbLogEnable = bool.Parse(config["DbConfig:SqlLogEnable"]);
        private static readonly string projectName = config["DbConfig:ProjectName"];
        private static readonly string HostID = config["DbConfig:MainDBIP"];
        private static readonly string port = config["DbConfig:DbPort"];
        private static readonly string UserID = config["DbConfig:UserID"];
        private static readonly string Password = AesTool.Decrypt(config["DbConfig:Password"]);
        private static readonly bool PoolingEnable = bool.Parse(config["DbConfig:NpgSqlPoolingEnable"]);

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string masterDbConnString = $"PORT={port};HOST={HostID};PASSWORD={Password};USER ID={UserID};DATABASE={projectName}_{McsDbType.DBManager};Pooling={PoolingEnable}";

        public static SqlSugarScope McsDb { get => (SqlSugarScope)McsApp.McsServiceProvider.Resolve<ISqlSugarClient>(); }

        //主数据库注册
        public static void DBRegist()
        {
            McsApp.McsRegistry.AddSingleton<ISqlSugarClient>(s =>
             {
                 SqlSugarScope sqlSugar = new(new ConnectionConfig()
                 {
                     DbType = DbType.PostgreSQL,
                     ConnectionString = masterDbConnString,
                     IsAutoCloseConnection = true,
                     ConfigId = $"{projectName}_{McsDbType.DBManager}",

                     ConfigureExternalServices = new ConfigureExternalServices()
                     {
                         EntityService = (c, p) =>
                         {
                             if (p.IsPrimarykey == false && new NullabilityInfoContext().Create(c).WriteState is NullabilityState.Nullable)
                             {
                                 p.IsNullable = true;
                             }
                         }
                     }
                 },

                 db =>
                 {
                     db.Aop.OnLogExecuting = (sql, paras) =>
                     {
                         if (dbLogEnable)
                         {
                             logger.Debug($"MainDB-- {UtilMethods.GetNativeSql(sql, paras)}");
                         }
                     };

                     db.Aop.OnLogExecuted = (sql, paras) =>
                     {
                         if (!dbLogEnable) return;
                         logger.Debug($"DB:MainDB--Sql: {sql}-Time:{db.Ado.SqlExecutionTime.TotalMilliseconds}ms");
                     };

                     db.Aop.OnError = (exp) =>
                     {
                         var x = UtilMethods.GetNativeSql(exp.Sql, (SugarParameter[])exp.Parametres);
                         logger.Error($"MainDB--{x}");
                     };
                 });

                 return sqlSugar;
             });
        }

        //主数据库初始化
        public static void MasterDBInit()
        {
            DbCreate(McsDbType.DBManager, McsDb);
        }

        public static string GetConnString(McsDbType dbType)
        {
            string dbID = $"{projectName}_{dbType}";
            var con = $"PORT={port};HOST={HostID};PASSWORD={Password};USER ID={UserID};DATABASE={dbID};Pooling={PoolingEnable}";
            return con;
        }

        public static void AllForceDbInit()
        {
            DbCreate(McsDbType.DBManager, McsDb);
            DbCreate(McsDbType.CurrentDB, DbRef(McsDbType.CurrentDB));
            DbCreate(McsDbType.SysConfigDB, DbRef(McsDbType.SysConfigDB));
            DbCreate(McsDbType.HistoryDB, DbRef(McsDbType.HistoryDB));
        }

        public static void ForceDBInit(McsDbType dbType)
        {
            DbCreate(dbType, DbRef(dbType));
        }

       

        //获取数据库引用
        public static ISqlSugarClient DbRef(McsDbType dbType, DateTime date = default, bool isScope = false)
        {
            ISqlSugarClient client = null;
            bool IsInit = false;
            string dbID = $"{projectName}_{dbType}";
            if (dbType == McsDbType.HistoryDB)
            {
                if (date == default) dbID = $"{dbID}{DateTime.Now.Year}";
                else dbID = $"{dbID}{date.Year}";
            }

            if (!mDbBuffer.TryGetValue(dbID, out DbConfigInfo config))
            {
                config = McsDb.Queryable<DbConfigInfo>().Where(a => a.ConfigID == dbID).First();
                if (config != null) mDbBuffer.TryAdd(dbID, config);
            }

            //连接不存在,则创建连接
            if (config == null)
            {
                config = new DbConfigInfo
                {
                    ConfigID = dbID,
                    DbType = dbType,
                    ConnectString = $"PORT={port};HOST={HostID};PASSWORD={Password};USER ID={UserID};DATABASE={dbID};Pooling={PoolingEnable}"
                };
                McsDb.Insertable(config).ExecuteCommand();
                IsInit = mDbBuffer.TryAdd(dbID, config);
            }
            client = GetDbRef(isScope, config);
            if (IsInit)
            {
                DbCreate(dbType, client);
                logger.Debug("{db}--创建", dbID);
            }
            return client;
        }

        //跨多库,历史数据查询
        public static ISqlSugarClient[] HistoryDBRefs(bool isScope = false)
        {
            List<ISqlSugarClient> clients = [];
            var configs = McsDb.Queryable<DbConfigInfo>().Where(a => a.DbType == McsDbType.HistoryDB).ToList();

            foreach (var config in configs)
            {
                var dbref = GetDbRef(isScope, config);
                clients.Add(dbref);
            }
            return [.. clients];
        }

        private static void DbCreate(McsDbType dbType, ISqlSugarClient client)
        {
            client.DbMaintenance.CreateDatabase();
            var tables = CodeFirstUtils.GetTables(dbType);
            client.CodeFirst.InitTables(tables);
        }

        private static ISqlSugarClient GetDbRef(bool isScope, DbConfigInfo config)
        {
            bool isAddAop = false;
            if (!McsDb.IsAnyConnection(config.ConfigID))
            {
                McsDb.AddConnection(new ConnectionConfig()
                {
                    ConfigId = config.ConfigID,
                    DbType = DbType.PostgreSQL,
                    ConnectionString = config.ConnectString,
                    IsAutoCloseConnection = true,
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityService = (c, p) =>
                        {
                            if (p.IsPrimarykey == false && new NullabilityInfoContext().Create(c).WriteState is NullabilityState.Nullable)
                            {
                                p.IsNullable = true;
                            }
                        }
                    }
                });
                isAddAop = true;
            }
            ISqlSugarClient client;
            if (isScope) client = McsDb.GetConnectionScope(config.ConfigID);
            else client = McsDb.GetConnection(config.ConfigID);
            if (isAddAop)
            {
                client.Aop.DataExecuted = (sql, paras) =>
                {
                    if (!dbLogEnable) return;
                    logger.Info($"Db:{config.ConfigID}--Sql:{sql}-Time:{client.Ado.SqlExecutionTime.TotalMilliseconds}ms");
                };

                client.Aop.OnLogExecuting = (sql, paras) =>
                {
                    if (dbLogEnable)
                    {
                        //获取原始SQL语句
                        logger.Debug($"{config.ConfigID}--{UtilMethods.GetNativeSql(sql, paras)}");
                    }
                };

                client.Aop.OnError = (exp) =>
                {
                    var x = UtilMethods.GetNativeSql(exp.Sql, (SugarParameter[])exp.Parametres);
                    logger.Error($"{config.ConfigID}--{x}");
                };
            }
            return client;
        }
    }
}