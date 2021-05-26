
using DC.Domain.Emuns;

namespace DC.Domain.Global
{
    /// <summary>
    /// 返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> where T : class
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_statusCode"></param>
        /// <param name="_success"></param>
        /// <param name="_msg"></param>
        public ApiResult(StatusCodeEnum _statusCode = StatusCodeEnum.OK, bool _success = true, string _msg = null)
        {
            StatusCode = _statusCode;
            Success = _success;

            if (StatusCode == StatusCodeEnum.OK)
            {
                Msg = StatusCode.ToDescriptionOrString();
            }
            else if (StatusCode == StatusCodeEnum.Error)
            {
                Msg = StatusCode.ToDescriptionOrString();
            }
            else if (_statusCode == StatusCodeEnum.Waring)
            {
                Msg = _msg;
            }
            else
            {
                Msg = StatusCode.ToDescriptionOrString();
            }
        }
       
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCodeEnum StatusCode { get; set; } = StatusCodeEnum.OK;

        /// <summary>
        /// 数据集
        /// </summary>
        public T Data { get; set; }
    }
}
