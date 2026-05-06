using McsCoreLib.Core.DBCore;

namespace McsCoreLib.Models.UserAuth
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class AuthLevel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AuthLevelId { get; set; }

        [SugarColumn(Length = 200)]
        public string AuthLevelName { get; set; }

        [SugarColumn(IsJson = true, ColumnDescription = "菜单资源列表")]
        public List<ResListItem> Resources { get; set; } = [];
    }

    /// <summary>
    /// 资源
    /// </summary>
    public class ResListItem
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public long MenuID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
    }
}