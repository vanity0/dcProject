namespace DC.Api.JwtAuth
{
    /// <summary>
    /// token
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 所属项目 必填
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 所属身份
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public double Expires { get; set; }

        /// <summary>
        /// 签发者 颁发机构 必填
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 颁发给谁 必填
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 秘钥 必填
        /// </summary>
        public string SecretKey { get; set; }
    }
}
