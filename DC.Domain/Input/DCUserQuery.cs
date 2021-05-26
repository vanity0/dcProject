using DC.Domain.Global;

namespace DC.Domain.Input
{
    public class DCUserQuery : PageQuery
    {
        public string Name { get; set; }
        /// <summary>
        /// 所属角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string ParentId { get; set; }

    }
}
