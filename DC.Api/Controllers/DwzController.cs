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
using Coldairarrow.Util;
using DC.Api.Extensions;
using System.Linq;
using DC.Domain.Output;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 短网址模块
    /// </summary>
    [Route("api/[controller]")]
    public class DwzController : BaseApiController
    {
        private readonly IDwzService _dwzService;
        private readonly IDCUserService _dCUserService;
        private readonly IProductService _productService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="dwzService"></param>
        public DwzController(IDwzService dwzService, IDCUserService dCUserService, IProductService productService)
        {
            _dwzService = dwzService;
            _dCUserService = dCUserService;
            _productService = productService;
        }

        /// <summary>
        /// 复制所有链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("getAllDwzUrl"), ApiFilter(Controller = "Dwz", Action = "GetAllDwzUrl")]
        public async Task<ApiResult<List<DwzOutput>>> GetAllDwzUrl()
        {
            var model = await _dCUserService.GetModelAsync(m => m.Id == AdminId && m.Status);
            if (model != null)
            {
                return new ApiResult<List<DwzOutput>>() { Data = await _dwzService.GetAllDwzUrlAsync(model.Id) };
            }
            else
            {
                return new ApiResult<List<DwzOutput>>(StatusCodeEnum.Waring, false, "用户被禁用!");
            }
        }

        /// <summary>
        /// 复制链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("getDwzUrl"), ApiFilter(Controller = "Dwz", Action = "AddDwz")]
        public async Task<ApiResult<string>> AddDwz([FromBody] DwzInput input)
        {
            var model = await _productService.GetModelAsync(m => m.Id == input.Id && m.AuditStatus == "上架中");
            if (model != null)
            {
                var dwzModel = await _dwzService.GetModelAsync(m => m.ProductId == model.Id && m.DCUserId == AdminId);
                if (dwzModel == null)
                {
                    var dwz = new Dwz()
                    {
                        Id = IdHelper.GetId(),
                        EncrypCodeParams = CodeUtils.GenerateCode(6),
                        ProductId = model.Id,
                        DCUserId = AdminId
                    };

                    var res = await _dwzService.AddAsync(dwz);
                    if (res > 0)
                    {
                        return new ApiResult<string>() { Data = dwz.EncrypCodeParams };
                    }
                    return new ApiResult<string>(StatusCodeEnum.Error, false);
                }
                else
                {
                    return new ApiResult<string>() { Data = dwzModel.EncrypCodeParams };
                }
            }
            else
            {
                return new ApiResult<string>(StatusCodeEnum.Waring, false, "该产品不存在!");
            }
        }
    }
}
