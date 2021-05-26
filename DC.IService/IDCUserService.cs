using System.Threading.Tasks;
using DC.Domain.Global;
using DC.IService;
using DC.Domain;
using DC.Domain.Input;
using DC.Domain.Output;
using System.Collections.Generic;
using DC.Domain.Models;

namespace DC.IService
{
    public interface IDCUserService : IBaseService<DCUser>
    {
        /// <summary>
        /// 获取所有员工
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        Task<ApiResult<PageModel<DCUserOutput>>> QueryAllDCUser(DCUserQuery query, string myUserId, bool async = true);

        /// <summary>
        /// 获取所有下级员工
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<PageModel<DCUserOutput>>> QueryDCUserChilder(DCUserQuery query, bool async = true);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<int> DeleteUserAsync(List<string> ids);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name=""></param>
        /// <param name="dwzs"></param>
        /// <returns></returns>
        Task<int> AddDcUserAsync(DCUser dCUser, List<Dwz> dwzs);

        /// <summary>
        /// 获取详情信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResult<DCUserOutput>> GetModelAsync(string id);
    }
}
