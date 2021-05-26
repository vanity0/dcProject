using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain;
using DC.Service;
using DC.IService;
using DC.Domain.Global;
using DC.Domain.Input;
using System;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class PvHistoryService : BaseService<PvHistory>, IPvHistoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public PvHistoryService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        public async Task<ApiResult<PageModel<PvHistory>>> GetPagesAsync(PvHistoryQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<PvHistory>>();

            var query = _fsql.Select<PvHistory>().OrderBy(m => m.CreateTime);


            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<PvHistory>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList() : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync()
            };
            return res;
        }

        public async Task<long> GetTotalCountAsync(ReportQuery reportQuery, string productId)
        {
            return await _fsql.Select<PvHistory>().Where(m => m.ProductId == productId)
                              .WhereIf(reportQuery.Date != null, m => m.CreateTime.Date == reportQuery.Date)
                              .WhereIf(reportQuery.Date == null, m => m.CreateTime.Date == DateTime.Now.Date)
                              .CountAsync();
        }
    }
}
