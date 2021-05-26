using System.Threading.Tasks;
using DC.Domain.Global;
using DC.IService;
using DC.Domain;
using DC.Domain.Input;
using DC.Domain.Output;
using System.Collections.Generic;

namespace DC.IService
{
    public interface IUvHistoryService : IBaseService<UvHistory>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<PageModel<UvHistorycpaCps>>> GetPagesAsync(UvHistoryQuery query, bool async = true);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="register"></param
        /// <param name="cpaHistory"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<int> AddModelrAsync(Register register, UvHistory uvHistory, bool async = true);

        Task<List<ReportLine>> GetReportByToday();

        Task<List<ReportLine>> GetReportDevByToday(string  userId);

        Task<long> GetTotalCountAsync(ReportQuery reportQuery, string productId, string systemType = null);
    }
}
