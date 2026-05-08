namespace CoreServiceLib.Models.ManulInfo
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class ManulControlType
    {
        [SugarColumn(ColumnDescription ="手动控制控件编号", IsPrimaryKey =true)]
        public int ManulTypeID {  get; set; }

        [SugarColumn(ColumnDescription = "手动控制控件名称",Length =100)]
        public string ManulTypeName { get; set; }
    }
}
