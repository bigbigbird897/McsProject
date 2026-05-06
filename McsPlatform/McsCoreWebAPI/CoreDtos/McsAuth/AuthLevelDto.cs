using McsCoreLib.Models.UserAuth;

namespace McsCoreInterface.CoreDtos.McsAuth
{
    /// <summary>
    ///  权限等级
    /// </summary>
    public class AuthLevelDto
    {
        /// <summary>
        /// 权限等级ID
        /// </summary>
        public int AuthLevelId { get; set; }

        /// <summary>
        /// 权限等级名称
        /// </summary>
        public string AuthLevelName { get; set; }

        /// <summary>
        /// 权限资源列表
        /// </summary>
        public List<ResListItem> Resources { get; set; } = [];
    }
}