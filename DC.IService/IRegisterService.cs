using System.Threading.Tasks;
using DC.Domain.Global;
using DC.IService;
using DC.Domain;
using DC.Domain.Input;
using DC.Domain.Output;

namespace DC.IService
{
    public interface IRegisterService : IBaseService<Register>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        Task<ApiResult<PageModel<RegisterOutput>>> GetPagesAsync(RegisterQuery query, bool async = true);

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<RegisterOutput>> GetModelByIdAsync(string id, bool async = true);

        Task<long> GetTotalCountAsync(ReportQuery reportQuery, string productId);
    }
}
