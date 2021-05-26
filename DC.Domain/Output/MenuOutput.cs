using System.Collections.Generic;

namespace DC.Domain.Output
{
    public class MenuOutput : Menu
    {
        /// <summary>
        /// 唯一性Key 必须
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; } = false;

        /// <summary>
        /// 子集 必须
        /// </summary>
        public List<MenuOutput> Children { get; set; }
    }
}
