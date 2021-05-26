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
using DC.Utils.Security;
using DC.Api.Base;
using DC.Domain.Models;
using DC.Api.Extensions;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 用户模块
    /// </summary>
    [Route("api/[controller]")]
    public class DCUserController : BaseApiController
    {
        private readonly IDCUserService _dcUserService;
        private readonly IProductService _productService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="dcUserService"></param>
        public DCUserController(IDCUserService dcUserService, IProductService productService)
        {
            _dcUserService = dcUserService;
            _productService = productService;
        }


        /// <summary>
        /// 获取所有下级员工，除开自己
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryDCUserChilder"), ApiFilter(Controller = "DCUser", Action = "QueryDCUserChilder")]
        public async Task<ApiResult<PageModel<DCUserOutput>>> QueryDCUserChilder([FromQuery] DCUserQuery input)
        {
            return await _dcUserService.QueryDCUserChilder(input);
        }


        /// <summary>
        /// 获取所有员工，除开自己
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryAllDCUser"), ApiFilter(Controller = "DCUser", Action = "QueryAllDCUser")]
        public async Task<ApiResult<PageModel<DCUserOutput>>> QueryDCUser([FromQuery] DCUserQuery input)
        {
            var userModel = await _dcUserService.GetModelAsync(m => m.Id == AdminId);
            if (userModel != null)
            {
                input.RoleId = userModel.RoleId;
                return await _dcUserService.QueryAllDCUser(input, AdminId);
            }
            return new ApiResult<PageModel<DCUserOutput>>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("addDCUser"), ApiFilter(Controller = "DCUser", Action = "AddDCUser")]
        public async Task<ApiResult<string>> AddDCUser([FromBody] DCUser input)
        {
            input.Id = IdHelper.GetId();
            input.CreateUser = Account;
            input.CreateTime = DateTime.Now;
            input.Pwd = input.Pwd.ToMD5Encrypt32();

           
            if (input.RoleId == "二级推手")
            {
                input.ParentId = AdminId;
            }

            var dwzList = new List<Dwz>();
            if (input.RoleId == "一级推手" || input.RoleId == "二级推手")
            {
                // 获取产品列表
                var prodList = await _productService.GetListAsync(m => m.AuditStatus == "上架中");
                foreach (var item in prodList)
                {
                    var dwz = new Dwz()
                    {
                        Id = IdHelper.GetId(),
                        EncrypCodeParams = CodeUtils.GenerateCode(6),
                        ProductId = item.Id,
                        DCUserId = input.Id
                    };
                    dwzList.Add(dwz);
                }
            }
            var res = await _dcUserService.AddDcUserAsync(input, dwzList);
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
        [HttpPost("deleteDCUser"), ApiFilter(Controller = "DCUser", Action = "DeleteDCUser")]
        public async Task<ApiResult<string>> DeleteDCUser([FromBody] List<string> ids)
        {
            if (ids.Contains(AdminId))
            {
                return new ApiResult<string>(StatusCodeEnum.Waring, false, "不可删除自己");
            }
            var exists = await _dcUserService.ExistsAsync(m => ids.Contains(m.ParentId));
            if (exists)
            {
                return new ApiResult<string>(StatusCodeEnum.Waring, false, "该用户下有下级，不可删除");
            }

            var res = await _dcUserService.DeleteUserAsync(ids);
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
        [HttpGet("getDCUser"), ApiFilter(Controller = "DCUser", Action = "GetDCUser")]
        public async Task<ApiResult<DCUserOutput>> GetDCUser([FromQuery] string id)
        {
            return await _dcUserService.GetModelAsync(id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("updateDCUser"), ApiFilter(Controller = "DCUser", Action = "UpdateDCUser")]
        public async Task<ApiResult<string>> UpdateDCUser([FromBody] DCUser input)
        {
            var model = await _dcUserService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "该数据不存在");
            }

            input.UpdateTime = DateTime.Now;
            input.UpdateUser = Account;

            var res = await _dcUserService.UpdateAsync((m) => new DCUser()
            {
                Name = input.Name,
                UpdateTime = DateTime.Now,
                UpdateUser = Account
            }, m => m.Id == model.Id);

            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("resetPwd"), ApiFilter(Controller = "DCUser", Action = "ResetPwd")]
        public async Task<ApiResult<string>> ResetPwd([FromBody] ResetPwdInput input)
        {
            var model = await _dcUserService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "该用户不存在");
            }
            if (string.IsNullOrEmpty(input.NewPwd))
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "新密码不可为空");
            }

            var res = await _dcUserService.UpdateAsync((m) => new DCUser()
            {
                Pwd = input.NewPwd.ToMD5Encrypt32(),
                UpdateTime = DateTime.Now,
                UpdateUser = Account
            }, m => m.Id == model.Id);

            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

    }
}