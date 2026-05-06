using System.ComponentModel;

namespace McsCoreInterface.CoreDtos.McsAuth
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户权限等级
        /// </summary>
        public int AuthLevelId { get; set; }

        /// <summary>
        /// 用户禁用
        /// </summary>
        ///
        [DefaultValue(false)]
        public bool Disabled { get; set; } = false;
    }
}