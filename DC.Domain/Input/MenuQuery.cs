using DC.Domain.Global;
using DC.Domain.Models;
using System.Collections.Generic;

namespace DC.Domain.Input
{
    public class MenuQuery : PageQuery
    {

    }

    /// <summary>
    /// 菜单添加/修改参数
    /// </summary>
    public class MenuInput : Menu
    {
        /// <summary>
        /// 按钮列表
        /// </summary>
        public List<MenuOperation> Operations { get; set; }
    }
}
