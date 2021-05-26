using System.Collections.Generic;

namespace DC.Domain.Global
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageModel<T>
    {
        /// <summary>
		/// 当前页索引
		/// </summary>
		public long PageNo { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPage { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 每页的记录数
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> Data { get; set; }
    }
}
