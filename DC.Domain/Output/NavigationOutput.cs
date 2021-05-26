using System.Collections.Generic;

namespace DC.Domain.Output
{
    /// <summary>
    /// 权限菜单
    /// </summary>
    public class NavigationOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限标示
        /// </summary>
        public string PermissionKey { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 前端路径
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public MetaDetail Meta { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<NavigationOutput> Children { get; set; }

        /// <summary>
        /// 页面操作权限
        /// </summary>
        public List<string> Buttons { get; set; }
    }

    /// <summary>
    /// 菜单详情
    /// </summary>
    public class MetaDetail
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否隐藏当前菜单
        /// </summary>
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// 是否隐藏头部内容
        /// </summary>
        public bool HiddenHeaderContent { get; set; } = false;

        /// <summary>
        /// 是否隐藏子菜单
        /// </summary>
        public bool HideChildren { get; set; } = false;
    }
}
