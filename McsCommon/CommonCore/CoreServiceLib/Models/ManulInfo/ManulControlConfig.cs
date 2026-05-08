using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Models.ManulInfo
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class ManulControlConfig
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "记录编号")]
        public int MID { get; set; }

        [SugarColumn(ColumnDescription = "类型编号")]
        public int ManulTypeID { get; set; }

        [SugarColumn(ColumnDescription = "控件名称", Length = 200)]
        public string ManulControlName { get; set; }

        [SugarColumn(ColumnDescription = "页编号")]
        public int PageNumber { get; set; }

        [SugarColumn(ColumnDescription = "当前页行号")]
        public int RowNumber { get; set; }
        [SugarColumn(ColumnDescription = "当前页列号")]
        public int ColNumber { get; set; }

        [SugarColumn(Length = 50, ColumnDescription = "设备编号")]
        public string DeviceId { get; set; }

        [SugarColumn(ColumnDescription = "标题", IsJson = true, IsNullable = true)]
        public JObject ManulTitles { get; set; }

        [SugarColumn(ColumnDescription = "动作参数", IsJson = true, IsNullable = true)]
        public JObject ManulActionPara { get; set; }

        [SugarColumn(ColumnDescription = "输出参数", IsJson = true, IsNullable = true)]
        public JObject OutputParas { get; set; }

        [SugarColumn(ColumnDescription = "禁用")]
        public bool IsDisable { get; set; }
    }
}
