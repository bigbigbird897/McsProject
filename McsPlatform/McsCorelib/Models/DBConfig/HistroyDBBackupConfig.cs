namespace McsCoreLib.Models.DBConfig
{
    [McsDbType(McsDbType.DBManager)]
    public class HistroyDBBackupConfig
    {
        [SugarColumn(IsPrimaryKey = true, Length = 100, ColumnDescription = "数据库ID")]
        public string ConfigID { get; set; }

        [SugarColumn(Length = 20, ColumnDescription = "备份服务器IP地址")]
        public string ServerIP { get; set; }

        [SugarColumn(ColumnDescription = "备份服务器端口")]
        public int ServerPort { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "备份服务器用户名")]
        public string UserName { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "备份服务器密码")]
        public string Password { get; set; }
    }
}