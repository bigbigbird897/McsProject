namespace McsCoreInterface.CoreDtos.McsAuth
{
    /// <summary>
    ///  用户登录结果
    /// </summary>
    public class LoginResultDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }



        /// <summary>
        /// 规则编号
        /// </summary>
        public int RoleId {  get; set; }


        /// <summary>
        ///   登录验证结果
        /// </summary>
        public bool CheckResult { get; set; } = false;


        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMessage { get; set; } = "";

        /// <summary>
        /// TOKEN
        /// </summary>
        public string LoginToken { get; set; } = "";

      
    }
}