using DC.Domain.Global;
using System;

namespace DC.Domain.Input
{
    public class UvHistoryQuery : PageQuery
    {
        /// <summary>
        /// 接入模式 CPS/CPA
        /// </summary>
        public string LinkModel { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? Time { get; set; }
    }
}
