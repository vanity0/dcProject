using System;

namespace DC.Api.Models
{

    /// <summary>
    /// 用户登录次数和过期时间配置
    /// </summary>
    public class CacheAdminLogin
    {
        /// <summary>
        /// 登录次数
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// 过期时间-分钟
        /// </summary>
        public DateTime? DelayMinute { get; set; }
    }
}
