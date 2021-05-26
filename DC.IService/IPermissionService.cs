using System.Threading.Tasks;
using DC.Domain.Global;
using DC.IService;
using DC.Domain;
using DC.Domain.Input;
using System.Collections.Generic;
using DC.Domain.Output;

namespace DC.IService
{
    public interface IPermissionService : IBaseService<Permission>
    {
        /// <summary>
        /// 根据按钮主键获取详细信息
        /// </summary>
        /// <param name="operationIds"></param>
        /// <returns></returns>
        Task<List<string>> GetOperationPermission(List<string> operationIds);

        /// <summary>
        /// 获取树形表格权限列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<PermissionOutput>>> GetTreeTableList(string roleId);

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync(List<Permission> input, string roleId);


        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="roleId">当前登录用户所属角色</param>
        /// <returns></returns>
        Task<ApiResult<List<NavigationOutput>>> GetNavigationList(string roleId);
    }
}
