namespace McsCoreLib.Models.DBConfig
{
    [McsDbType(McsDbType.DBManager)]
    public class DbConfigInfo
    {
        [SugarColumn(IsPrimaryKey = true, Length = 100)]
        public string ConfigID { get; set; }

        public McsDbType DbType { get; set; }

        [SugarColumn(Length = 300)]
        public string ConnectString { get; set; }

        [SugarColumn(InsertServerTime = true)]
        public DateTime CreateTime { get; set; }
    }
}