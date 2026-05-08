using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Models.PosCalConfig
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class AreaDataConfig
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "区域编号")]
        public int AreaID { get; set; }
        [SugarColumn(Length = 200, ColumnDescription = "区域名称")]
        public string AreaName { get; set; }

        //依据区域类型解析参数
        [SugarColumn(ColumnDescription = "区域定义参数",IsNullable =true)]
        public JObject AreaPara { get; set; }

        //标定点定义列表
        [SugarColumn(ColumnDescription = "标定点定义", IsJson = true)]
        public List<CalPosDef> CalPosDatas { get; set; } = [];

        //标定点计算结果列表
        [SugarColumn(ColumnDescription = "实际标定点数据", IsJson = true)]
        public List<PosCalData> PosCalDatas { get; set; } = [];
    }
}
