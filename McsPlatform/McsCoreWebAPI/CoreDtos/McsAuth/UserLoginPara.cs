namespace McsCoreInterface.CoreDtos.McsAuth
{
    /// <summary>
    /// 用户登录参数
    /// </summary>
    public class UserLoginPara
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
    }
}