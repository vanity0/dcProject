using System.ComponentModel;

namespace DC.Domain.Emuns
{

     
    /// <summary>
    /// 返回状态
    /// </summary>
    public enum StatusCodeEnum
    {
        /// <summary>
        /// 指示客服端的请求已经成功收到，解析，接受
        /// </summary>
        [Description("操作成功")]
        OK = 200,
        /// <summary>
        /// 操作提示
        /// </summary>
        [Description("操作提示")]
        Waring = 203,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Error = 300,

        /// <summary>
        /// 因为错误的语法导致服务器无法理解请求信息。
        /// </summary>
        [Description("因为错误的语法导致服务器无法理解请求信息。")]
        BadRequest = 400,
        /// <summary>
        /// 如果请求需要用户验证。回送应该包含一个WWW-Authenticate头字段用来指明请求资源的权限。
        /// </summary>
        [Description("如果请求需要用户验证。回送应该包含一个WWW-Authenticate头字段用来指明请求资源的权限。")]
        Unauthorized = 401,
        /// <summary>
        /// 服务器接受请求，但是被拒绝处理。
        /// </summary>
        [Description("服务器接受请求，但是被拒绝处理。")]
        Forbidden = 403,
        /// <summary>
        /// 服务器已经找到任何匹配Request-URI的资源。
        /// </summary>
        [Description("服务器已经找到任何匹配Request-URI的资源。")]
        NotFound = 404,
        /// <summary>
        /// Request-Line请求的方法不被允许通过指定的URI。
        /// </summary>
        [Description("Request-Line请求的方法不被允许通过指定的URI。")]
        MenthodNotAllowed = 405,
        /// <summary>
        /// 服务器遭遇异常阻止了当前请求的执行
        /// </summary>
        [Description("服务器遭遇异常阻止了当前请求的执行")]
        InternalServerError = 500,
    }
}
