using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DC.Api.Extensions
{
    public class UrlMappingMiddleWare
    {
        private readonly RequestDelegate _next;
        public UrlMappingMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.HasValue)
            {
                var pathValue = path.Value;
                if (pathValue.StartsWith("/")) pathValue = pathValue.Substring(1);
                if (Regex.IsMatch(pathValue, @"^[a-zA-Z0-9]{6}$"))
                {
                    #region 生成短网址,url地址后面跟 生成的code, 然后根据生成的code获取原来的url地址，进行跳转
                    var code = CodeUtils.GenerateCode(6);
                    var longUrl = "";// await urlMappingRepository.GetUrlByCode(pathValue);
                    #endregion


                    if (!string.IsNullOrEmpty(longUrl))
                    {
                        if (!Regex.IsMatch(longUrl, "(file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://", RegexOptions.IgnoreCase))
                        {
                            longUrl = "http://" + longUrl;
                        }
                        context.Response.Redirect(longUrl, true);
                        return;
                    }
                }
            }
            await _next.Invoke(context);
        }

    }

    public static class CodeUtils
    {
        static readonly Random random = new Random();
        static readonly char[] charsArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        static readonly int charsArrayLength = charsArray.Length;

        public static string GenerateCode(int length)
        {
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = charsArray[random.Next(charsArrayLength)];
            }

            return new string(result);
        }

        public static bool HasValue<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any()) return true;
            return false;
        }
    }
}
