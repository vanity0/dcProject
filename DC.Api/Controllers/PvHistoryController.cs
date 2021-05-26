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
    /// PV访问量
    /// </summary>
    [Route("api/[controller]")]
    public class PvHistoryController : BaseApiController
    {
        private readonly IDwzService _dwzService;
        private readonly IProductService _productService;
        private readonly IPvHistoryService _pvHistoryService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="pvHistoryService"></param>
        public PvHistoryController(IPvHistoryService pvHistoryService, IDwzService dwzService,IProductService productService)
        {
            _pvHistoryService = pvHistoryService;
            _productService = productService;
            _dwzService = dwzService;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryPvHistory"), ApiFilter(Controller = "PvHistory", Action = "QueryPvHistory")]
        public async Task<ApiResult<PageModel<PvHistory>>> QueryPvHistory([FromQuery] PvHistoryQuery input)
        {
            return await _pvHistoryService.GetPagesAsync(input);
        }

        /// <summary>
        /// 单个删除、批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("deletePvHistory"), ApiFilter(Controller = "PvHistory", Action = "DeletePvHistory")]
        public async Task<ApiResult<string>> DeletePvHistory([FromBody] List<string> ids)
        {
            var res = await _pvHistoryService.DeleteAsync(m => ids.Contains(m.Id));
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 根据复制链接的加密短码获取产品信息，计算PV
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("getProductByEnCode"), AnyFilter(Controller = "PvHistory", Action = "GetProductByEnCode")]
        public async Task<ApiResult<ProductDevOutput>> GetProductByEnCode([FromBody] ProductDevInput c)
        {
            if (!string.IsNullOrEmpty(c.C))
            {
                var dwz = await _dwzService.GetModelAsync(m => m.EncrypCodeParams == c.C);
                if (dwz != null)
                {
                    var prod = await _productService.GetModelAsync(m => m.Id == dwz.ProductId);
                    if (prod != null)
                    {
                        // 记录 Pv
                        var pvHistory = new PvHistory()
                        {
                            Id = IdHelper.GetId(),
                            CreateTime = DateTime.Now,
                            ProductId = prod.Id,
                            DCUserId = dwz.DCUserId,
                            SystemType = c.M,
                            Ip = this.HttpContext.GetTrueIP(), //context.HttpContext.Connection.RemoteIpAddress.ToString(),
                        };

                        var exists = await _pvHistoryService.ExistsAsync(m => m.ProductId == prod.Id && m.Ip == pvHistory.Ip);
                        if (!exists)
                        {
                            await _pvHistoryService.AddAsync(pvHistory);
                        }

                        return new ApiResult<ProductDevOutput>()
                        {
                            Data = new ProductDevOutput()
                            {
                                Id = prod.Id,
                                Logo = prod.Logo,
                                Name = prod.Name,
                                Tag = prod.Tag,
                                Type = prod.ProductType,
                                UId = dwz.DCUserId
                            }
                        };
                    }
                }
            }

            return new ApiResult<ProductDevOutput>()
            {
                Msg = "拒绝处理",
                StatusCode = StatusCodeEnum.Forbidden
            };
        }

    }
}