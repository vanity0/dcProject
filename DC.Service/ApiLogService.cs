using FreeSql;
using System.Threading.Tasks;
using DC.IService;
using DC.Domain.Models;
using DC.Domain.Global;
using DC.Domain.Input;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiLogService : BaseService<ApiLog>, IApiLogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public ApiLogService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageModel<ApiLog>>> GetPagesAsync(ApiLogQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<ApiLog>>();

            var query = _fsql.Select<ApiLog>()
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Key), m =>
                                 m.RequestUrl.Contains(pageQuery.Key) ||
                                 m.RequestParams.Contains(pageQuery.Key) ||
                                 m.CreateUser.Contains(pageQuery.Key))
                             .WhereIf(pageQuery.startDate != null && pageQuery.endDate != null, m => m.CreateTime >= pageQuery.startDate && m.CreateTime <= pageQuery.endDate);

            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<ApiLog>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList<ApiLog>() : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync<ApiLog>()
            };
            return res;
        }
    }
}
