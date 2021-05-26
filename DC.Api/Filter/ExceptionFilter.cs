using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DC.Utils.IOC;
using DC.Utils;
using DC.Api.Models;
using DC.Domain.Global;
using Coldairarrow.Util;
using DC.Domain.Models;
using DC.Api.Extensions;

namespace DC.Api.Filter
{
    /// <summary>
    /// 全局异常捕捉器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 重写异常拦截日志
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var msg = $"\n 地址：{UserModel.Url} \n " +
                 $"用户：{UserModel.Account}\n " +
                 $"来源：{context.Exception.Source}\n " +
                 $"堆栈：{context.Exception.StackTrace}\n " +
                 $"异常：{context.Exception.Message}";

            //MethodBase.GetCurrentMethod().Name
            AutofacHelper.GetScopeService<INLogService>().Error(msg);

            var res = AutofacHelper.GetScopeService<IService.IErrorLogService>().AddAsync(new ErrorLog()
            {
                Id = IdHelper.GetId(),
                CreateUser = UserModel.Account,
                CreateTime = DateTime.Now,
                ClassName = "",
                ExceptionName = "",
                MethodName = "",
                RequestParams = context.ActionDescriptor.Parameters.ToString(),
                RequestTime = DateTime.Now,
                UserAgent = context.HttpContext.Request.Headers["User-Agent"],
                RemoteIp = context.HttpContext.GetTrueIP(), //context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Message = UserModel.Token,
                StackTrace = msg
            });

            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(new ApiResult<string>()
                {
                    StatusCode = Domain.Emuns.StatusCodeEnum.Forbidden,
                    Success = false,
                    Msg = context.Exception.Message
                }),
                ContentType = "application/json; charset=utf-8",
            };
        }
    }
}
