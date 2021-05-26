using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DC.IService;
using DC.Api.Filter;
using DC.Domain.Global;
using DC.Domain.Models;
using DC.Domain.Input;
using DC.Domain.Emuns;
using DC.Api.Base;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 异常日志模块
    /// </summary>
    [Route("api/[controller]")]
    public class ErrorLogController : BaseApiController
    {
        private readonly IErrorLogService _errorLogService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="errorLogService"></param>
        public ErrorLogController(IErrorLogService errorLogService)
        {
            _errorLogService = errorLogService;
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryErrorLog"), ApiFilter(Controller = "ErrorLog", Action = "QueryErrorLog")]
        public async Task<ApiResult<PageModel<ErrorLog>>> QueryErrorLog([FromQuery]ErrorLogQuery input)
        {
            return await _errorLogService.GetPagesAsync(input);
        }

        /// <summary>
        /// 根据ID主键获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getErrorLog"), ApiFilter(Controller = "ErrorLog", Action = "GetErrorLog")]
        public async Task<ApiResult<ErrorLog>> GetErrorLog([FromQuery] string id)
        {
            return new ApiResult<ErrorLog>()
            {
                Data = await _errorLogService.GetModelAsync(m => m.Id == id)
            };
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteErrorLog"), ApiFilter(Controller = "ErrorLog", Action = "deleteErrorLog")]
        public async Task<ApiResult<string>> DeleteErrorLog([FromBody]List<string> ids)
        {
            var res = await _errorLogService.DeleteAsync(m => ids.Contains(m.Id));
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }
    }
}
