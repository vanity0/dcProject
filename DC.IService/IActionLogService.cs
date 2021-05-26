using System.Threading.Tasks;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Models;

namespace DC.IService
{
    /// <summary>
    /// 系统日志接口
    /// </summary>
    public interface IActionLogService : IBaseService<ActionLog>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<PageModel<ActionLog>>> GetPagesAsync(ActionLogQuery pageQuery, bool async = true);
    }
}
