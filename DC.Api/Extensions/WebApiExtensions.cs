using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Api.Extensions
{
    public static class WebApiExtensions
    {
        /// <summary>
        /// 获取真实客户端ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetTrueIP(this HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var trueip = context.Request.Headers["CF-Connecting-IP"].ToString();
            if (!string.IsNullOrEmpty(trueip) && ip != trueip)
            {
                ip = trueip;
            }
            return ip;
        }
    }
}
