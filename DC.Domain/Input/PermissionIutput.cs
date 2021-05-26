using DC.Domain.Output;
using System.Collections.Generic;

namespace DC.Domain.Input
{
    public class PermissionIutput
    {
        /// <summary>
        /// 所属角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<PermissionOutput> Permissions { get; set; }
    }
}
