using DC.Domain.Global;
using System;

namespace DC.Domain.Input
{
    /// <summary>
    /// 日志查询列表参数
    /// </summary>
    public class ApiLogQuery : PageQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endDate { get; set; }
    }
}
