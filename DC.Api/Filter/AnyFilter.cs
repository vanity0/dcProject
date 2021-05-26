using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Web;
using System.Text;
using System.Diagnostics;
using DC.Utils.IOC;
using DC.Api.JwtAuth;
using DC.Api.Models;
using DC.Domain.Global;
using DC.Domain.Emuns;
using DC.Utils;
using Coldairarrow.Util;
using DC.Utils.IPUtils;
using DC.Domain.Models;
using DC.IService;
using DC.Api.Extensions;

namespace DC.Api.Filter
{
    /// <summary>
    /// Pv记录
    /// </summary>
    public class AnyFilter : ActionFilterAttribute
    {

        #region 字段和属性

        /// <summary>
        /// 模块别名，可配置更改
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 权限动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 权限访问控制器参数
        /// </summary>
        private string Sign { get; set; }

        /// <summary>
        /// 是否保存日志
        /// </summary>
        public bool IsLog { get; set; } = true;

        private string ActionArguments { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        private Stopwatch Stopwatch { get; set; }

        #endregion

        /// <summary>
        /// 在Controller的Action方法执行前，但是Action方法的参数模型绑定完成后执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (IsLog)
            {
                ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
                Stopwatch = new Stopwatch();
                Stopwatch.Start();
            }
            // var tokenValid = context.HttpContext.Request.Cookies["Email"].MDString3(AppConfig.BaiduAK).Equals(context.HttpContext.Request.Cookies["FullAccessToken"]);

            #region 限制IP访问地址

            //拦截掉黑名单中的IP
            //var request = context.HttpContext.Request;
            //var ip = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //if (ip.IsDenyIpAddress())
            //{
            //    AccessDeny(ip, request, "黑名单IP地址");
            //    ContextReturn(context, "您当前所在的网络环境不支持访问本站");
            //    return;
            //}


            //if (request.Headers[HeaderNames.UserAgent].ToString().Contains(AppSetting.Get("UserAgentBlocked").Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries)))
            //{
            //    AccessDeny(ip, request, "UA黑名单");
            //    ContextReturn(context, "您当前所在的浏览器不支持访问本站");
            //    return;
            //}

            //拦截特定的IP地址段
            //if (IPAddressFilter.IsInDenyArea(ip))
            //{

            //    AccessDeny(ip, request, "访问地区限制");
            //    ContextReturn(context, "您当前所在的地区不支持访问本站站");
            //    return;
            //}

            //if (Regex.IsMatch(request.Method, "OPTIONS|HEAD", RegexOptions.IgnoreCase) || request.IsRobot())
            //{
            //    return;
            //}

            //限制请求频次300次/分钟
            //var times = CacheManager.AddOrUpdate("Frequency:" + ip, 1, i => i + 1, 5);
            //CacheManager.Expire("Frequency:" + ip, ExpirationMode.Sliding, TimeSpan.FromSeconds(CommonHelper.SystemSettings.GetOrAdd("LimitIPFrequency", "60").ToInt32()));
            //var limit = CommonHelper.SystemSettings.GetOrAdd("LimitIPRequestTimes", "90").ToInt32();
            //if (times <= limit)
            //{
            //    return;
            //}

            //if (times > limit * 1.2)
            //{
            //    CacheManager.Expire("Frequency:" + ip, ExpirationMode.Sliding, TimeSpan.FromMinutes(CommonHelper.SystemSettings.GetOrAdd("BanIPTimespan", "10").ToInt32()));
            //    AccessDeny(ip, request, "访问频次限制");
            //}

            //拦截请求中包含敏感词的流量
            //var path = HttpUtility.UrlDecode(request.Path + request.QueryString, Encoding.UTF8);
            //if (Regex.Match(path ?? "", "彩票|办证|AV女优").Length > 0) // 写死的敏感词，实际项目请从本地库中动态读取
            //{
            //    // todo:记录拦截日志
            //    context.Result = new BadRequestObjectResult("参数不合法！");
            //    return;
            //}
            #endregion

          
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 返回API的信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        private void ContextReturn(ActionExecutingContext context, string msg)
        {
            var res = new ApiResult<string>()
            {
                StatusCode = StatusCodeEnum.Unauthorized,
                Msg = msg
            };
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(res));
            context.Result = new EmptyResult();
        }

        /// <summary>
        /// 检查Ip地址是否在黑名单里面
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="request"></param>
        /// <param name="remark"></param>
        private void AccessDeny(string ip, HttpRequest request, string remark)
        {
            var path = HttpUtility.UrlDecode(request.Path + request.QueryString, Encoding.UTF8);
            //BackgroundJob.Enqueue(() => HangfireBackJob.InterceptLog(new IpIntercepter()
            //{
            //    IP = ip,
            //    RequestUrl = HttpUtility.UrlDecode(request.Scheme + "://" + request.Host + path),
            //    Time = DateTime.Now,
            //    UserAgent = request.Headers[HeaderNames.UserAgent],
            //    Remark = remark
            //}));
        }

        /// <summary>
        /// 在Controller的Action方法执行完后执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (IsLog)
            {
                Stopwatch.Stop();

                var account = "";
                var qs = ActionArguments;
                var method = context.HttpContext.Request.Method;
                var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

                 

                var msg = $"\n 方法：{Controller}：{Action} \n " +
                    $"地址：{url} \n " +
                    $"方式：{method} \n " +
                    $"参数：{qs}\n ";

                AutofacHelper.GetScopeService<INLogService>().Info(msg);

                AutofacHelper.GetScopeService<IApiLogService>().AddAsync(new ApiLog()
                {
                    Id = IdHelper.GetId(),
                    CreateUser = account,
                    ElapsedTime = Stopwatch.Elapsed.TotalMilliseconds,
                    CreateTime = DateTime.Now,
                    ClassName = Controller,
                    Method = method,
                    MethodName = Action,
                    RequestTime = DateTime.Now,
                    UserAgent = context.HttpContext.Request.Headers["User-Agent"],
                    RemoteIp = context.HttpContext.GetTrueIP(), //context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    RequestUrl = url,
                    RequestParams = qs
                });
            }

        }
    }
}
