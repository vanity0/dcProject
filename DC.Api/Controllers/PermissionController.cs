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
using System.Linq;
using DC.Api.Base;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 权限模块
    /// </summary>
    [Route("api/[controller]")]
    public class PermissionController : BaseApiController
    {
        private readonly IPermissionService _permissionService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="permissionService"></param>
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        /// <summary>
        /// 获取树形表格权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryPermission"), ApiFilter(Controller = "Permission", Action = "QueryPermission")]
        public async Task<ApiResult<List<PermissionOutput>>> QueryPermission([FromQuery] PermissionQuery query)
        {
            return await _permissionService.GetTreeTableList(query.RoleId);
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sysPermissionIutPut"></param>
        /// <returns></returns>
        [HttpPost("addPermission"), ApiFilter(Controller = "Permission", Action = "AddPermission")]
        public async Task<ApiResult<string>> AddPermission([FromBody] PermissionIutput sysPermissionIutPut)
        {
            var parentList = sysPermissionIutPut.Permissions.Where(m => m.Checked).ToList();
            var sysPermissions = GetPermissions(parentList, sysPermissionIutPut.RoleId);
            var res = await _permissionService.SaveAsync(sysPermissions, sysPermissionIutPut.RoleId);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        private List<Permission> GetPermissions(List<PermissionOutput> permissionIutputs, string roleId)
        {
            var list = new List<Permission>();
            foreach (var item in permissionIutputs)
            {
                if (item.Operations.Count > 0)
                {
                    foreach (var operation in item.Operations.Where(m => m.Checked))
                    {
                        list.Add(new Permission()
                        {
                            Id = IdHelper.GetId(),
                            CreateUser = Account,
                            CreateTime = DateTime.Now,
                            MenuId = item.Id,
                            RoleId = roleId,
                            OperationId = operation.Value
                        });
                    }
                }
                else
                {
                    var model = new Permission()
                    {
                        Id = IdHelper.GetId(),
                        CreateUser = Account,
                        CreateTime = DateTime.Now,
                        MenuId = item.Id,
                        RoleId = roleId
                    };
                    list.Add(model);
                }

                
                if (item.Children != null)
                {
                    list.AddRange(GetPermissions(item.Children.Where(m => m.Checked).ToList(), roleId));
                }
            }
            return list;
        }
    }
}