using CoreServiceLib.Core.DBCore;

namespace CoreServiceLib.Models.Material
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class MaterialTypeConfig
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "物料类型编号")]
        public int MaterialTypeID { get; set; }

        [SugarColumn(Length = 200, ColumnDescription = "物料类型名称")]
        public string MaterialTypeName { get; set; }
    }
}