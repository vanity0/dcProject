namespace DC.Domain.Input
{
    public class RegisterInput
    {
        /// <summary>
        /// 推手ID
        /// </summary>
        public string U { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string P { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string M { get; set; }

        /// <summary>
        /// 注册ID
        /// </summary>
        public string RegId { get; set; }
    }
}
