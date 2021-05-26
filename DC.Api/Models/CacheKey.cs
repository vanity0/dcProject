using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Api.Models
{
    /// <summary>
    /// 缓存Key
    /// </summary>
    public class CacheKey
    {
        #region 用户登录配置

        /// <summary>
        /// 用户登录保存用户方式
        /// </summary>
        public static string LoginSaveUser = "Login:SaveType";

        /// <summary>
        /// 用户登录保存Cookie过期时间  小时
        /// </summary>
        public static string LoginCookieExpires = "Login:ExpiresHours";

        /// <summary>
        /// 用户登录-保存登录次数
        /// </summary>
        public static string LoginCount = "Login:Count";

        /// <summary>
        /// 用户登录-延时分钟
        /// </summary>
        public static string LogindElayMinute = "Login:DelayMinute";
        #endregion
    }
}
