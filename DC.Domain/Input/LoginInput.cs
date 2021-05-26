namespace DC.Domain.Input
{
    /// <summary>
    /// 登录请求参数
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
