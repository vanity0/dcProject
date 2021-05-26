using DC.Api.Base;
using DC.Api.Filter;
using DC.Domain.Global;
using DC.Domain.Output;
using DC.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 首页模块
    /// </summary>
    [Route("api/[controller]")]
    public class HomeController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IPvHistoryService _pvHistoryService;
        private readonly IUvHistoryService _uvHistoryService;
        private readonly IRegisterService _registerService;
        private readonly IDCUserService _dCUserService;
        public HomeController(IPvHistoryService pvHistoryService, IUvHistoryService uvHistoryService,
            IRegisterService registerService, IDCUserService dCUserService, IProductService productService)
        {
            _pvHistoryService = pvHistoryService;
            _uvHistoryService = uvHistoryService;
            _registerService = registerService;
            _dCUserService = dCUserService;
            _productService = productService;
        }


        [HttpGet("queryDevReport"), ApiFilter(Controller = "Home", Action = "QueryDevReport")]
        public async Task<ApiResult<List<ReportLine>>> QueryDevReport()
        {
            return new ApiResult<List<ReportLine>>()
            {
                Data = await _uvHistoryService.GetReportDevByToday(AdminId)
            };
        }

        [HttpGet("queryDevBacklog"), ApiFilter(Controller = "Home", Action = "QueryDevBacklog")]
        public async Task<ApiResult<List<string>>> QueryDevBacklog()
        {
            var list = await _registerService.GetListAsync(m => m.CreateTime.Date == DateTime.Now.Date, 10);

            return new ApiResult<List<string>>()
            {
                Data = list.Select(m => m.CreateTime + "注册人:" + m.Name).ToList()
            };
        }


        [HttpGet("queryReport"), ApiFilter(Controller = "Home", Action = "QueryReport")]
        public async Task<ApiResult<List<ReportLine>>> QueryReport()
        {
            return new ApiResult<List<ReportLine>>()
            {
                Data = await _uvHistoryService.GetReportByToday()
            };
        }

        [HttpGet("queryBacklog"), ApiFilter(Controller = "Home", Action = "QueryBacklog")]
        public async Task<ApiResult<List<string>>> QueryBacklog()
        {
            var list = await _productService.GetListAsync(m => m.AuditStatus == "审核中",10);

            return new ApiResult<List<string>>()
            {
                Data = list.Select(m => m.CreateTime + "申请产品:" + m.Name).ToList()
            };
        }

        /// <summary>
        /// 获取总平台首页统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryDCCount"), ApiFilter(Controller = "Home", Action = "QueryDCCount")]
        public async Task<ApiResult<HomeTotalCount>> QueryDCCount()
        {
            // 总推客
            var devCount = await _dCUserService.GetTotalCountAsync(m => m.RoleId == "一级推手" || m.RoleId == "二级推手");

            // 产品总数
            var productCount = await _productService.GetTotalCountAsync(m => m.AuditStatus == "上架中");

            // 今日PV
            var pvCount = await _pvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date);

            // 今日UV
            var uvCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date);

            // 今日CPA
            var cpaCount = await _registerService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.Status == "待更新");

            // IOS/安卓
            var iosCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.SystemType.ToLower().Contains("ios"));
            var androidCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.SystemType.ToLower().Contains("android"));

            return new ApiResult<HomeTotalCount>()
            {
                Data = new HomeTotalCount()
                {
                    DevCount = devCount,
                    ProductCount = productCount,
                    PvCount = pvCount,
                    UvCount = uvCount,
                    CpaCount = cpaCount,
                    IosCount = iosCount,
                    AndroidCount = androidCount,
                }
            };
        }

        /// <summary>
        /// 获取商家首页统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryProxyCount"), ApiFilter(Controller = "Home", Action = "QueryProxyCount")]
        public async Task<ApiResult<HomeTotalCount>> QueryProxyCount()
        {
            // 根据商家所有上架推手端的产品计算
            var products = await _productService.GetListAsync(m => m.AuditStatus == "上架中");

            var ids = products.Select(m => m.Id);
            // 今日PV
            var pvCount = await _pvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && ids.Contains(m.ProductId));

            // 今日UV
            var uvCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && ids.Contains(m.ProductId));

            // 今日CPA
            var cpaCount = await _registerService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.Status == "待更新" && ids.Contains(m.ProductId));

            // IOS/安卓
            var iosCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.SystemType.ToLower().Contains("ios") && ids.Contains(m.ProductId));
            var androidCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.SystemType.ToLower().Contains("android") && ids.Contains(m.ProductId));

            return new ApiResult<HomeTotalCount>()
            {
                Data = new HomeTotalCount()
                {
                    PvCount = pvCount,
                    UvCount = uvCount,
                    CpaCount = cpaCount,
                    IosCount = iosCount,
                    AndroidCount = androidCount,
                }
            };
        }

        /// <summary>
        /// 获取推手首页统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("queryDevCount"), ApiFilter(Controller = "Home", Action = "QueryDevCount")]
        public async Task<ApiResult<HomeTotalCount>> QueryDevCount()
        {
            // 根据推手ID 统计数量
            // 今日PV
            var pvCount = await _pvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.DCUserId == AdminId);

            // 今日UV
            var uvCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.DCUserId == AdminId);

            // 今日CPA
            var cpaCount = await _registerService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.Status == "待更新" && m.DCUserId == AdminId);

            // IOS/安卓
            var iosCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.SystemType.ToLower().Contains("ios") && m.DCUserId == AdminId);
            var androidCount = await _uvHistoryService.GetTotalCountAsync(m => m.CreateTime.Date == DateTime.Now.Date && m.SystemType.ToLower().Contains("android") && m.DCUserId == AdminId);

            return new ApiResult<HomeTotalCount>()
            {
                Data = new HomeTotalCount()
                {
                    PvCount = pvCount,
                    UvCount = uvCount,
                    CpaCount = cpaCount,
                    IosCount = iosCount,
                    AndroidCount = androidCount,
                }
            };
        }
    }
}
