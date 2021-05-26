namespace DC.Domain.Output
{
    public class RegisterOutput : Register
    {
        /// <summary>
        /// 所属推手名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 所属产品名称
        /// </summary>
        public string ProductName { get; set; }
    }
}
