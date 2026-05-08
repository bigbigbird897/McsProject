using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Models.Hardware
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class LineConfig
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "产线编号")]
        public string LineID { get; set; }

        [SugarColumn(Length = 400, ColumnDescription = "产线名称")]
        public string LineName { get; set; }

        [SugarColumn(IsJson = true, IsNullable = true, ColumnDescription = "产线公共参数")]
        public JObject LinePara { get; set; }
    }
}