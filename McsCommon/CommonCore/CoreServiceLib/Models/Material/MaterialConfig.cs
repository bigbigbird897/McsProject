using CoreServiceLib.Core.DBCore;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Models.Material
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class MaterialConfig
    {
        [SugarColumn(IsPrimaryKey = true, Length = 100, ColumnDescription = "物料编号")]
        public string MaterialID { get; set; }

        [SugarColumn(Length = 300, IsNullable = true, ColumnDescription = "物料名称")]
        public string MaterialName { get; set; }

        [SugarColumn(ColumnDescription = "物料类型编号")]
        public int MaterialTypeID { get; set; }

        [SugarColumn(IsJson = true, IsNullable = true, ColumnDescription = "物料扩展参数")]
        public JObject MaterialTypePara { get; set; }

        [SugarColumn(ColumnDescription = "物料禁用")]
        public bool IsDisable { get; set; }

        [SugarColumn(ColumnDescription = "创建日期", InsertServerTime = true)]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnDescription = "修改日期", UpdateServerTime = true, IsNullable = true)]
        public DateTime UpdateTime { get; set; }

        [SugarColumn(ColumnDescription = "创建人", IsNullable = true, Length = 50)]
        public string Creator { get; set; }

        [SugarColumn(ColumnDescription = "修改人", IsNullable = true, Length = 50)]
        public string Modifier { get; set; }
    }
}