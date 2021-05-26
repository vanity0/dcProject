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
using DC.Api.Extensions;
using DC.Domain.Output;

namespace DC.Api.Controllers
{
    /// <summary>
    /// UV点击量
    /// </summary>
    [Route("api/[controller]")]
    public class UvHistoryController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IUvHistoryService _uvHistoryService;
        private readonly IRegisterService _registerService;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="uvHistoryService"></param>
        public UvHistoryController(IUvHistoryService uvHistoryService, IProductService productService, IRegisterService registerService)
        {
            _uvHistoryService = uvHistoryService;
            _productService = productService;
            _registerService = registerService;
        }

        /// <summary>
        /// 获取cps/cpa,UV数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryUvHistory"), ApiFilter(Controller = "UvHistory", Action = "QueryUvHistory")]
        public async Task<ApiResult<PageModel<UvHistorycpaCps>>> QueryUvHistory([FromQuery] UvHistoryQuery input)
        {
            return await _uvHistoryService.GetPagesAsync(input);
        }

        /// <summary>
        /// 提交注册计算UV
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("addUvHistory"), AnyFilter(Controller = "UvHistory", Action = "AddUvHistory")]
        public async Task<ApiResult<UvHistoryOutput>> AddUvHistory([FromBody] UvHistoryInput input)
        {
            var prod = await _productService.GetModelAsync(m => m.Id == input.P);
            if (prod != null)
            {
                var ip = this.HttpContext.GetTrueIP();//context.HttpContext.Connection.RemoteIpAddress.ToString(),

                #region 组装UV
                var uvHistory = new UvHistory();
                var exists = await _uvHistoryService.ExistsAsync(m => m.ProductId == prod.Id && m.Ip == ip && m.DCUserId == input.U);
                if (!exists)
                {
                    uvHistory = new UvHistory()
                    {
                        Id = IdHelper.GetId(),
                        CreateTime = DateTime.Now,
                        Ip = ip,
                        DCUserId = input.U,
                        ProductId = input.P,
                        SystemType = input.M,
                    };
                }

                #endregion

                #region 组装推广数据
                var register = new Register();
                var existsRegister = await _registerService.ExistsAsync(m => m.ProductId == prod.Id && m.Ip == ip && m.DCUserId == input.U  && m.Tel == input.Phone && m.Name == input.Name && m.CreateTime.Date == DateTime.Now.Date);
                if (!existsRegister)
                {
                    register = new Register()
                    {
                        Id = IdHelper.GetId(),
                        CreateTime = DateTime.Now,
                        Status = "待更新",
                        Ip = ip,
                        Name = input.Name,
                        Tel = input.Phone,
                        ProductId = input.P,
                        DCUserId = input.U,
                        SystemType = input.M,
                    };
                }
                #endregion

                var res = await _uvHistoryService.AddModelrAsync(register, uvHistory);
                if (res > 0)
                {
                    return new ApiResult<UvHistoryOutput>() { Data =new UvHistoryOutput() { Url = prod.Url, RegId = register.Id } };
                }
                return new ApiResult<UvHistoryOutput>() { Data =new UvHistoryOutput() { Url = prod.Url, RegId = register.Id } };
            }
            return new ApiResult<UvHistoryOutput>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 单个删除、批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deleteUvHistory"), ApiFilter(Controller = "UvHistory", Action = "DeleteUvHistory")]
        public async Task<ApiResult<string>> DeleteUvHistory([FromBody] List<string> ids)
        {
            var res = await _uvHistoryService.DeleteAsync(m => ids.Contains(m.Id));
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }
    }
}