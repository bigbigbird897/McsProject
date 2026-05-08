using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Models.Hardware
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class StationConfig
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "工位编号")]
        public string StationID { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "产线编号")]
        public string LineID { get; set; }

        [SugarColumn(Length = 400, ColumnDescription = "工位名称", IsNullable = true)]
        public string StationName { get; set; }

        [SugarColumn(ColumnDescription = "工位功能")]
        public int FuncTypeID { get; set; }

        [SugarColumn(IsJson = true, IsNullable = true, ColumnDescription = "工位参数")]
        public JObject StationPara { get; set; }

        public bool IsDisable { get; set; }
    }
}