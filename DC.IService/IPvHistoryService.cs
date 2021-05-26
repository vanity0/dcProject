using System.Threading.Tasks;
using DC.Domain.Global;
using DC.IService;
using DC.Domain;
using DC.Domain.Input;

namespace DC.IService
{
    public interface IPvHistoryService : IBaseService<PvHistory>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        Task<ApiResult<PageModel<PvHistory>>> GetPagesAsync(PvHistoryQuery query, bool async = true);

        Task<long> GetTotalCountAsync(ReportQuery reportQuery, string productId);
    }
}
