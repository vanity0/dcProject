using FreeSql.DataAnnotations;
using System;

namespace DC.Domain
{
    /// <summary>
    /// 权限
    /// </summary>
    [Table(Name = "Permission")]
    public class Permission
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
