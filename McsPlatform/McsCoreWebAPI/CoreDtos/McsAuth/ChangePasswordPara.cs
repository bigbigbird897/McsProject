namespace McsCoreInterface.CoreDtos.McsAuth
{
    /// <summary>
    /// 密码改变参数
    /// </summary>
    public class ChangePasswordPara
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; } = "";

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; } = "";

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; } = "";

        /// <summary>
        /// 确认新密码
        /// </summary>
        public string ConfirmPassword { get; set; } = "";
    }
}