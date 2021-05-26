using DC.Api.Base;
using DC.Api.Filter;
using DC.Domain;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Output;
using DC.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DC.Api.Controllers
{
    /// <summary>
    /// 数据统计模块
    /// </summary>
    [Route("api/[controller]")]
    public class ReportController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IPvHistoryService _pvHistoryService;
        private readonly IUvHistoryService _uvHistoryService;
        private readonly IRegisterService _registerService;
        private readonly IDCUserService _dCUserService;
        public ReportController(IPvHistoryService pvHistoryService, IUvHistoryService uvHistoryService,
            IRegisterService registerService, IDCUserService dCUserService, IProductService productService)
        {
            _pvHistoryService = pvHistoryService;
            _uvHistoryService = uvHistoryService;
            _registerService = registerService;
            _dCUserService = dCUserService;
            _productService = productService;
        }


        /// <summary>
        /// 按每个产品统计PV,UV,CPA等
        /// </summary>
        /// <returns></returns>
        [HttpGet("queryList"), ApiFilter(Controller = "Report", Action = "QueryList")]
        public async Task<ApiResult<List<ReportOutput>>> QueryList([FromQuery]ReportQuery reportQuery)
        {
            var list = new List<ReportOutput>();

            Expression<Func<Product, bool>> exp = m => m.AuditStatus == "上架中";

            if (!string.IsNullOrEmpty(reportQuery.Name))
            {
                exp.And(m => m.Name.Contains(reportQuery.Name));
            }
            var products = await _productService.GetListAsync(exp);


            foreach (var item in products)
            {
                var report = new ReportOutput();
                report.Name = item.Name;
                report.Pv = await _pvHistoryService.GetTotalCountAsync(reportQuery, item.Id);
                report.Uv = await _uvHistoryService.GetTotalCountAsync(reportQuery, item.Id);
                report.Cpa = await _registerService.GetTotalCountAsync(reportQuery, item.Id);
                report.IOS = await _uvHistoryService.GetTotalCountAsync(reportQuery, item.Id, "ios");
                report.Android = await _uvHistoryService.GetTotalCountAsync(reportQuery, item.Id, "android");
                list.Add(report);
            }

            return new ApiResult<List<ReportOutput>>()
            {
                Data = list
            };
        }

    }
}
