namespace CoreServiceLib.Models.PosCalConfig
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class ActionTypeInfo
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "动作类型编号")]
        public int ActionTypeID { get; set; }
        [SugarColumn(Length = 200, ColumnDescription = "动作类型名称")]
        public string ActionTypeName { get; set; }
    }
}
