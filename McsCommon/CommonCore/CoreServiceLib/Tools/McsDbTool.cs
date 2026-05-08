using CoreServiceLib.Core.IocExt;

namespace CoreServiceLib.Tools
{
    public class McsDbTool
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string GetConnStr(McsDbType dbType)
        {
            return DbManager.GetConnString(dbType);
        }

        /// <summary>
        /// 主数据库初始化
        /// </summary>
        public static void MasterDBInit()
        {
            DbManager.MasterDBInit();
        }

        /// <summary>
        /// 数据库注册
        /// </summary>
        public static void DBRegist()
        {
            DbManager.DBRegist();
        }

        /// <summary>
        /// 获取仓储连接
        /// </summary>
        /// <typeparam name="T">表</typeparam>
        /// <returns></returns>
        public static McsRepository<T> GetDBRepositoryRef<T>() where T : class, new()
        {
            return McsApp.McsServiceProvider.Resolve<McsRepository<T>>();
        }

        /// <summary>
        /// 用数据库表获取连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isScope"></param>
        /// <returns></returns>
        public static ISqlSugarClient GetDBRef<T>(bool isScope = false) where T : class, new()
        {
            ISqlSugarClient mref = null;
            typeof(T).GetCustomAttributes(typeof(McsDbTypeAttribute), false).ToList()
                           .ForEach(x =>
                           {
                               if (x is McsDbTypeAttribute mAtt)
                               {
                                   mref = DbManager.DbRef(mAtt.McsDB, default, isScope);
                                   return;
                               }
                           });
            return mref;
        }


        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="isScope"></param>
        /// <returns></returns>
        public static ISqlSugarClient GetRef(McsDbType dbType, bool isScope = false)
        {
            return DbManager.DbRef(dbType, default, isScope);
        }


        /// <summary>
        /// 原生Sql 调用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="isScope"></param>
        /// <returns></returns>

        public static IAdo SqlRef(McsDbType dbType, bool isScope = false)
        {
            return DbManager.DbRef(dbType, default, isScope).Ado;
        }

        /// <summary>
        /// 事务操作 --用using 语句包裹，using不能少不然意外会导致事务无法释放
        /// </summary>
        /// <returns></returns>
        public static SqlSugarTransaction McsTrans
        {
            get => DbManager.McsDb.UseTran();
        }

        /// <summary>
        /// 历史数据库跨库查询
        /// </summary>
        /// <returns></returns>
        public static ISqlSugarClient[] HistoryDBRefs
        {
            get => DbManager.HistoryDBRefs();
        }
    }
}
