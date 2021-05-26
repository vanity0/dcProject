using System.Threading.Tasks;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Models;

namespace DC.IService
{
    /// <summary>
    /// 系统日志接口
    /// </summary>
    public interface IApiLogService : IBaseService<ApiLog>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<PageModel<ApiLog>>> GetPagesAsync(ApiLogQuery pageQuery, bool async = true);
    }
}
