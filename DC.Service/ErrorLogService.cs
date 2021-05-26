using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Models;
using DC.IService;
using FreeSql;
using System.Threading.Tasks;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorLogService : BaseService<ErrorLog>, IErrorLogService
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public ErrorLogService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        public async Task<ApiResult<PageModel<ErrorLog>>> GetPagesAsync(ErrorLogQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<ErrorLog>>();

            var query = _fsql.Select<ErrorLog>()
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Key), m =>
                                 m.StackTrace.Contains(pageQuery.Key) ||
                                 m.Message.Contains(pageQuery.Key) ||
                                 m.CreateUser.Contains(pageQuery.Key))
                             .WhereIf(pageQuery.startDate != null && pageQuery.endDate != null, m => m.CreateTime >= pageQuery.startDate && m.CreateTime <= pageQuery.endDate);

            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<ErrorLog>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList<ErrorLog>() : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync<ErrorLog>()
            };
            return res;
        }
    }
}
