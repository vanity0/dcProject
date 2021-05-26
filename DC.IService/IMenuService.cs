using System.Threading.Tasks;
using DC.Domain.Global;
using DC.Domain;
using DC.Domain.Input;
using System.Collections.Generic;
using DC.Domain.Output;
using DC.Domain.Models;

namespace DC.IService
{
    public interface IMenuService : IBaseService<Menu>
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        Task<ApiResult<List<MenuOutput>>> GetTreeTablePagesAsync(MenuQuery query, bool async = true);

        /// <summary>
        /// 查询树列表
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        Task<ApiResult<List<TreeSelectOutPut>>> QueryMenuTreeSelect(string menuType);

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MenuInput> GetModelAsync(string id);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> AddAsync(MenuInput sysMenu, List<MenuOperation> list);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysMenu"></param>
        /// <param name="updateList"></param>
        /// <param name="newList"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<int> UpdateAsync(MenuInput sysMenu, List<MenuOperation> updateList, List<MenuOperation> newList);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteMenuAsync(List<string> id);
    }
}
