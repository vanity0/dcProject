using DC.Domain.Global;
using System;

namespace DC.Domain.Input
{
    public class RegisterQuery : PageQuery
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 当前推手用户
        /// </summary>
        public string DCUserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
