using FreeSql.DataAnnotations;
using System;

namespace DC.Domain.Models
{
    /// <summary>
    /// 菜单-按钮
    /// </summary>
    [Table(Name = "MenuOperation")]
    public class MenuOperation
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标识 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
