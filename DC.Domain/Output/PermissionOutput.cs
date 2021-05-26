using System;
using System.Collections.Generic;

namespace DC.Domain.Output
{
    public class PermissionOutput 
    {
        /// <summary>
        /// 菜单主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 唯一性Key 必须
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; } = false;

        /// <summary>
        /// 包含按钮
        /// </summary>
        public List<OperationListOutPut> Operations { get; set; }

        /// <summary>
        /// 子集 必须
        /// </summary>
        public List<PermissionOutput> Children { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 用于菜单列表显示以及权限列表显示操作按钮
    /// </summary>
    public class OperationListOutPut
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; } = false;
    }
}
