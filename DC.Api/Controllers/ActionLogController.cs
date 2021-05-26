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
    /// 操作日志模块
    /// </summary>
    [Route("api/[controller]")]
    public class ActionLogController : BaseApiController
    {
        private readonly IActionLogService _actionLogService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="actionLogService"></param>
        public ActionLogController(IActionLogService actionLogService)
        {
            _actionLogService = actionLogService;
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryActionLog"), ApiFilter(Controller = "ActionLog", Action = "QueryActionLog")]
        public async Task<ApiResult<PageModel<ActionLog>>> QueryActionLog([FromQuery]ActionLogQuery input)
        {
            return await _actionLogService.GetPagesAsync(input);
        }

        /// <summary>
        /// 根据ID主键获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getActionLog"), ApiFilter(Controller = "ActionLog", Action = "GetActionLog")]
        public async Task<ApiResult<ActionLog>> GetActionLog([FromQuery] string id)
        {
            return new ApiResult<ActionLog>()
            {
                Data = await _actionLogService.GetModelAsync(m => m.Id == id)
            };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteActionLog"), ApiFilter(Controller = "ActionLog", Action = "DeleteActionLog")]
        public async Task<ApiResult<string>> DeleteActionLog([FromBody]List<string> ids)
        {
            var res = await _actionLogService.DeleteAsync(m => ids.Contains(m.Id));
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }
    }
}
