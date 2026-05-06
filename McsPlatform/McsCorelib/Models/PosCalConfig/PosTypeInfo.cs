namespace McsCoreLib.Models.PosCalConfig
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class PosTypeInfo
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "点位类型编号")]
        public int PosTypeID { get; set; }
        [SugarColumn(Length = 200, ColumnDescription = "点位类型名称")]
        public string PosTypeName { get; set; }
    }
}
