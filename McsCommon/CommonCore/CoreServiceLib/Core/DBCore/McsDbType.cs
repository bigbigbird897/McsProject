namespace CoreServiceLib.Core.DBCore
{
    public enum McsDbType
    {
        DBManager,   //业务数据库管理器
        SysConfigDB,  //配置管理器
        CurrentDB,  //热数据库
        HistoryDB, //冷备数据库
        WorkFlowDB, //工作流数据库
        WorkJobDB// 后台任务数据库
    }
}