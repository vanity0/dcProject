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
using System.Linq;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 菜单按钮模块
    /// </summary>
    [Route("api/[controller]")]
    public class MenuOperationController : BaseApiController
    {
        private readonly IMenuOperationService _menuOperationService;
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="menuOperationService"></param>
        /// <param name="permissionService"></param>
        public MenuOperationController(IMenuOperationService menuOperationService, IPermissionService permissionService)
        {
            _menuOperationService = menuOperationService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryMenuOperation"), ApiFilter(Controller = "MenuOperation", Action = "QueryMenuOperation")]
        public async Task<ApiResult<List<MenuOperation>>> QueryMenuOperation([FromQuery] MenuOperationQuery input)
        {
            return await _menuOperationService.GetListAsync(input);
        }

        /// <summary>
        /// 单个删除、批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteMenuOperation"), ApiFilter(Controller = "MenuOperation", Action = "DeleteMenuOperation")]
        public async Task<ApiResult<string>> DeleteMenuOperation([FromBody] List<string> ids)
        {
            var list = await _permissionService.GetOperationPermission(ids);

            if (list.Count > 0)
            {
                return new ApiResult<string>(StatusCodeEnum.Waring, false, "有数据被其他信息使用,不可删除！[" + string.Join(",", list) + "]");
            }
            else
            {
                var res = await _menuOperationService.DeleteAsync(m => ids.Contains(m.Id));
                if (res > 0)
                {
                    return new ApiResult<string>();
                }
                return new ApiResult<string>(StatusCodeEnum.Error, false);
            }
        }
    }
}