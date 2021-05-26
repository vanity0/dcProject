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
using DC.Domain.Models;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 产品模块
    /// </summary>
    [Route("api/[controller]")]
    public class ProductController : BaseApiController
    {
        private readonly IDwzService _dwzService;
        private readonly IProductService _productService;
        private readonly IDCUserService _dCUserService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="productService"></param>
        public ProductController(IProductService productService, IDCUserService dCUserService, IDwzService dwzService)
        {
            _productService = productService;
            _dCUserService = dCUserService;
            _dwzService = dwzService;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryProduct"), ApiFilter(Controller = "Product", Action = "QueryProduct")]
        public async Task<ApiResult<PageModel<Product>>> QueryProduct([FromQuery] ProductQuery input)
        {
            if (RoleId == "商家")
            {
                input.Account = Account;
            }
            return await _productService.GetPagesAsync(input);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("sortProduct"), ApiFilter(Controller = "Product", Action = "SortProduct")]
        public async Task<ApiResult<string>> SortProduct([FromBody] SortProductInput input)
        {
            var model = await _productService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Waring, false, "该数据不存在");
            }
            var sortNum = model.Sort;
            if (input.HasUp)
            {// 上移 1 最顶部
                if (model.Sort > 2)
                {
                    model.Sort -= 1;
                }
                else
                {
                    return new ApiResult<string>(StatusCodeEnum.Waring, false, "已经是第一位");
                }
            }
            else
            {// 下移 总数最后一个最底下
                var count = await _productService.GetTotalCountAsync(m => true);
                if (model.Sort < count)
                {
                    model.Sort += 1;
                }
                else
                {
                    return new ApiResult<string>(StatusCodeEnum.Waring, false, "已经是最后一位");
                }
            }
            var updateProduct = await _productService.GetModelAsync(m => m.Sort == model.Sort);
            updateProduct.Sort = sortNum;

            var res = await _productService.SortProductAsync(model, updateProduct);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 获取推手端展示的产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryProductDev"), ApiFilter(Controller = "Product", Action = "QueryProductDev")]
        public async Task<ApiResult<ProdDevOutput>> QueryProductDev([FromQuery] ProductQuery input)
        {
            var res = new ApiResult<ProdDevOutput>();

            var model = await _dCUserService.GetModelAsync(m => m.Id == AdminId && m.Status);
            if (model != null)
            {
                res.Data = new ProdDevOutput()
                {
                    PorductList = await _productService.GetPagesByDevAsync(input, AdminId),
                    DwzList = await _dwzService.GetAllDwzUrlAsync(model.Id)
                };
                return res;
            }
            else
            {
                return new ApiResult<ProdDevOutput>(StatusCodeEnum.Waring, false, "用户被禁用!");
            }
        }

        /// <summary>
        /// 获取H5端展示的产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpGet("queryProductH5"), AnyFilter(Controller = "Product", Action = "QueryPagesProductH5")]
        public async Task<ApiResult<PageModel<ProductH5Output>>> QueryPagesProductH5([FromQuery] ProductQuery input)
        {
            return await _productService.QueryPagesProductH5(input);
        }

        /// <summary>
        /// 获取推广数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryProductList"), ApiFilter(Controller = "Product", Action = "QueryProductList")]
        public async Task<ApiResult<List<Product>>> QueryProductList()
        {
            return await _productService.QueryProductListAsync(Account);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("addProduct"), ApiFilter(Controller = "Product", Action = "AddProduct")]
        public async Task<ApiResult<string>> AddProduct([FromBody] Product input)
        {
            var maxModel = await _productService.GetMaxModelAsync(m => true, m => m.Sort);
            input.Sort = maxModel.Sort + 1;
            input.Id = IdHelper.GetId();
            input.CreateTime = DateTime.Now;
            input.CreateUser = Account;
            var res = await _productService.AddAsync(input);
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
        [HttpPost("deleteProduct"), ApiFilter(Controller = "Product", Action = "DeleteProduct")]
        public async Task<ApiResult<string>> DeleteProduct([FromBody] List<string> ids)
        {
            var res = await _productService.DeleteProdAsync(ids);
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
        [HttpGet("getProduct"), ApiFilter(Controller = "Product", Action = "GetProduct")]
        public async Task<ApiResult<Product>> GetProduct([FromQuery] string id)
        {
            return new ApiResult<Product>()
            {
                Data = await _productService.GetModelAsync(m => m.Id == id)
            };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("updateProduct"), ApiFilter(Controller = "Product", Action = "UpdateProduct")]
        public async Task<ApiResult<string>> UpdateProduct([FromBody] Product input)
        {
            var model = await _productService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "该数据不存在");
            }

            input.UpdateUser = Account;
            input.UpdateTime = DateTime.Now;
            var res = await _productService.UpdateAsync(input);
            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }


        /// <summary>
        /// 上架申请、审核通过、审核不通过、上架、下架
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("setProductStatus"), ApiFilter(Controller = "Product", Action = "SetProductStatus")]
        public async Task<ApiResult<string>> SetProductStatus([FromBody] SetProductInput input)
        {
            var model = await _productService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "该数据不存在");
            }


            model.AuditStatus = input.AuditStatus;
            model.UpdateTime = DateTime.Now;
            model.UpdateUser = Account;

            if (input.AuditStatus == "审核中")
            {
                model.ExtendMoney = input.ExpandMoney;
            }

            var dwzList = new List<Dwz>();
            if (input.AuditStatus == "上架中")
            {// 生成该产品所有推广人员的推广链接

                if (string.IsNullOrEmpty(model.Url))
                {
                    return new ApiResult<string>(StatusCodeEnum.Waring, false, "该产品地址不存在，不可通过审核");
                }

                var userList = await _dCUserService.GetListAsync(m => m.RoleId == "一级推手" || m.RoleId == "二级推手");
                foreach (var item in userList)
                {
                    var dwz = new Dwz()
                    {
                        Id = IdHelper.GetId(),
                        EncrypCodeParams = CodeUtils.GenerateCode(6),
                        ProductId = model.Id,
                        DCUserId = item.Id
                    };
                    dwzList.Add(dwz);
                }
            }

            var res = await _productService.UpdateProduAsync(model, dwzList);

            if (res > 0)
            {
                return new ApiResult<string>();
            }
            return new ApiResult<string>(StatusCodeEnum.Error, false);
        }

        /// <summary>
        /// 设置H5上架，下架
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("setProducth5"), ApiFilter(Controller = "Product", Action = "SetProducth5")]
        public async Task<ApiResult<string>> SetProducth5([FromBody] Product input)
        {
            var model = await _productService.GetModelAsync(m => m.Id == input.Id);
            if (model == null)
            {
                return new ApiResult<string>(StatusCodeEnum.Error, false, "该数据不存在");
            }
            var res = await _productService.UpdateAsync((m) => new Product()
            {
                H5 = input.H5,
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