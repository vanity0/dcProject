using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.IService;
using DC.Domain.Models;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Output;

namespace DC.Service
{

    /// <summary>
    /// 
    /// </summary>
    public class MenuOperationService : BaseService<MenuOperation>, IMenuOperationService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public MenuOperationService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<MenuOperation>>> GetListAsync(MenuOperationQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<List<MenuOperation>>();

            var query = _fsql.Select<MenuOperation>()
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Name), m => m.Name.Contains(pageQuery.Name))
                             .Where(m => m.MenuId == pageQuery.MenuId)
                             .OrderBy(m => m.CreateTime);

            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = async ? query.ToList<MenuOperation>() : await query.ToListAsync<MenuOperation>();

            return res;
        }
    }
}
