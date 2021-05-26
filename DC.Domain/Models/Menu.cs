using FreeSql.DataAnnotations;
using System;

namespace DC.Domain
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Table(Name = "Menu")]
    public class Menu
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 前端页面具体位置
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 菜单图标 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 所属父级
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 菜单类型 
        /// </summary>
        public string MenuType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

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
