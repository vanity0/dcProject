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
using DC.Api.Base;
using DC.Domain.Output;
using DC.Api.Extensions;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 推广数据模块
    /// </summary>
    [Route("api/[controller]")]
    public class RegisterController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IRegisterService _registerService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="registerService"></param>
        public RegisterController(IRegisterService registerService, IProductService productService)
        {
            _registerService = registerService;
            _productService = productService;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryRegister"), ApiFilter(Controller = "Register", Action = "QueryRegister")]
        public async Task<ApiResult<PageModel<RegisterOutput>>> QueryRegister([FromQuery] RegisterQuery input)
        {
            if (RoleId != "平台" && RoleId!="商家")
            {
                input.DCUserId = AdminId;
            }
            
            return await _registerService.GetPagesAsync(input);

        }

        /// <summary>
        /// 单个删除、批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteRegister"), ApiFilter(Controller = "Register", Action = "DeleteRegister")]
        public async Task<ApiResult<string>> DeleteRegister([FromBody] List<string> ids)
        {
            var res = await _registerService.DeleteAsync(m => ids.Contains(m.Id));
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
        [HttpGet("getRegister"), ApiFilter(Controller = "Register", Action = "GetRegister")]
        public async Task<ApiResult<RegisterOutput>> GetRegister([FromQuery] string id)
        {
            return await _registerService.GetModelByIdAsync(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("addRegister"), AnyFilter(Controller = "Register", Action = "AddRegister")]
        public async Task<ApiResult<string>> AddRegister([FromBody] RegisterInput input)
        {
            var prod = await _productService.GetModelAsync(m => m.Id == input.P);
            if (prod != null)
            {
                var ip = this.HttpContext.GetTrueIP();//context.HttpContext.Connection.RemoteIpAddress.ToString(),

                var register = await _registerService.GetModelAsync(m => m.Id == input.RegId &&
                                        m.ProductId == input.P &&
                                        m.Ip == ip &&
                                        m.DCUserId == input.U &&
                                        m.Status == "待更新" &&
                                        m.CreateTime.Date == DateTime.Now.Date);
                if (register != null)
                {
                    register.Status = "审核中";
                    var res = await _registerService.UpdateAsync(register);
                    if (res > 0)
                    {
                        return new ApiResult<string>();
                    }
                }
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("updateRegister"), ApiFilter(Controller = "Register", Action = "UpdateRegister")]
        public async Task<ApiResult<string>> UpdateRegister([FromBody] Register input)
        {
            var res = await _registerService.UpdateAsync(input);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 审核通过、拒绝通过
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("setRegisterStatus"), ApiFilter(Controller = "Register", Action = "SetRegisterStatus")]
        public async Task<ApiResult<string>> SetRegisterStatus([FromBody] Register input)
        {
            var model = await _registerService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "该数据不存在");
            }

            var res = await _registerService.UpdateAsync((m) => new Register()
            {
                Status = input.Status,
                AuditTime = DateTime.Now,
                AuditUser = Account
            }, m => m.Id == model.Id);

            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }
    }
}