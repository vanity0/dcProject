using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DC.Domain.Global;
using DC.Api.Filter;
using Coldairarrow.Util;
using DC.Domain.Emuns;
using DC.Domain;
using DC.Domain.Input;
using DC.IService;
using DC.Domain.Output;
using DC.Api.Base;
using DC.Domain.Models;
using System.Linq;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 菜单模块
    /// </summary>
    [Route("api/[controller]")]
    public class MenuController : BaseApiController
    {
        private readonly IMenuService _menuService;
        private readonly IMenuOperationService _menuOperationService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="menuService"></param
        /// <param name="menuOperationService"></param>
        public MenuController(IMenuService menuService, IMenuOperationService menuOperationService)
        {
            _menuService = menuService;
            _menuOperationService = menuOperationService;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryMenu"), ApiFilter(Controller = "Menu", Action = "QueryMenu")]
        public async Task<ApiResult<List<MenuOutput>>> QueryMenu([FromQuery] MenuQuery input)
        {
            return await _menuService.GetTreeTablePagesAsync(input);
        }

        /// <summary>
        /// 获取树形下拉
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        [HttpGet("queryMenuTreeSelect"), ApiFilter(Controller = "Menu", Action = "QueryMenuTreeList")]
        public async Task<ApiResult<List<TreeSelectOutPut>>> QueryMenuTreeSelect([FromQuery] string menuType)
        {
            return await _menuService.QueryMenuTreeSelect(menuType);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("addMenu"), ApiFilter(Controller = "Menu", Action = "AddMenu")]
        public async Task<ApiResult<string>> AddMenu([FromBody] MenuInput input)
        {
            input.Id = IdHelper.GetId();
            input.CreateUser = Account;
            input.CreateTime = DateTime.Now;

            var list = new List<MenuOperation>();
            if (input.Operations != null)
            {
                for (int i = 0; i < input.Operations.Count; i++)
                {
                    list.Add(new MenuOperation()
                    {
                        Id = IdHelper.GetId(),
                        CreateTime = input.CreateTime,
                        CreateUser = input.CreateUser,
                        MenuId = input.Id,
                        Code = input.Operations[i].Code,
                        Name = input.Operations[i].Name,
                    });
                }
            }

            var res = await _menuService.AddAsync(input, list);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 单个删除、批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteMenu"), ApiFilter(Controller = "Menu", Action = "DeleteMenu")]
        public async Task<ApiResult<string>> DeleteMenu([FromBody] List<string> ids)
        {
            var res = await _menuService.DeleteMenuAsync(ids);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 根据ID主键获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getMenu"), ApiFilter(Controller = "Menu", Action = "GetMenu")]
        public async Task<ApiResult<MenuInput>> GetMenu([FromQuery] string id)
        {
            return new ApiResult<MenuInput>()
            {
                Data = await _menuService.GetModelAsync(id)
            };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("updateMenu"), ApiFilter(Controller = "Menu", Action = "UpdateMenu")]
        public async Task<ApiResult<string>> UpdateMenu([FromBody] MenuInput input)
        {
            var updateList = new List<MenuOperation>();
            var newList = new List<MenuOperation>();
            if (input.Operations != null)
            {
                // 1. 获取已经存在的修改  
                var ids = input.Operations.Select(o => o.Id);
                var oldList = await _menuOperationService.GetListAsync(m => ids.Contains(m.Id));
                updateList = input.Operations.Where(o => oldList.Any(old => old.Id == o.Id && (old.Name != o.Name || old.Code != o.Code))).ToList();

                // 2. 获取要新增的
                newList = input.Operations.Where(o => !oldList.Any(old => old.Id == o.Id)).ToList();
                newList.ForEach(m =>
                {
                    m.Id = IdHelper.GetId();
                    m.CreateTime = input.CreateTime;
                    m.CreateUser = input.CreateUser;
                    m.MenuId = input.Id;

                });

            }
            input.UpdateTime = DateTime.Now;
            input.UpdateUser = Account;
            var res = await _menuService.UpdateAsync(input, updateList, newList);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }
    }
}