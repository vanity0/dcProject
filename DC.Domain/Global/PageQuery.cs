
namespace DC.Domain.Global
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PageQuery
    {
        /// <summary>
        /// 关键字查找
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 当前第几页
        /// </summary>
        public int Current { get; set; } = 1;

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
