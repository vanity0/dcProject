using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Models;
using DC.Domain.Output;

namespace DC.IService
{
    /// <summary>
    /// 按钮操作接口
    /// </summary>
    public interface IMenuOperationService : IBaseService<MenuOperation>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<List<MenuOperation>>> GetListAsync(MenuOperationQuery pageQuery, bool async = true);
    }
}
