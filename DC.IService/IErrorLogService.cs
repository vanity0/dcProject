using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Models;
using System.Threading.Tasks;

namespace DC.IService
{
    /// <summary>
    /// 异常日志接口
    /// </summary>
    public interface IErrorLogService : IBaseService<ErrorLog>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<PageModel<ErrorLog>>> GetPagesAsync(ErrorLogQuery pageQuery, bool async = true);
    }
}
