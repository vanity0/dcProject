using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DC.Domain.Global;
using DC.Api.Filter;
using DC.Domain.Emuns;
using DC.Domain.Models;
using DC.Domain.Input;
using DC.IService;
using DC.Api.Base;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 请求日志模块
    /// </summary>
    [Route("api/[controller]")]
    public class ApiLogController : BaseApiController
    {
        private readonly IApiLogService _apiLogService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="apiLogService"></param>
        public ApiLogController(IApiLogService apiLogService)
        {
            _apiLogService = apiLogService;
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryApiLog"), ApiFilter(Controller = "ApiLog", Action = "QueryApiLog")]
        public async Task<ApiResult<PageModel<ApiLog>>> QueryApiLog([FromQuery]ApiLogQuery input)
        {
            return await _apiLogService.GetPagesAsync(input);
        }

        /// <summary>
        /// 根据ID主键获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getApiLog"), ApiFilter(Controller = "ApiLog", Action = "GetApiLog")]
        public async Task<ApiResult<ApiLog>> GetApiLog([FromQuery] string id)
        {
            return new ApiResult<ApiLog>()
            {
                Data = await _apiLogService.GetModelAsync(m => m.Id == id)
            };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteApiLog"), ApiFilter(Controller = "ApiLog", Action = "DeleteApiLog")]
        public async Task<ApiResult<string>> DeleteApiLog([FromBody]List<string> ids)
        {
            var res = await _apiLogService.DeleteAsync(m => ids.Contains(m.Id));
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }
    }
}
