namespace DC.Domain.Output
{
    public class DCUserOutput : DCUser
    {
        /// <summary>
        /// 所属角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 上级用户账户
        /// </summary>
        public string ParentName { get; set; }
    }
}
