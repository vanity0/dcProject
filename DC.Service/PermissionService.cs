using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain;
using DC.Service;
using DC.IService;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Output;
using System.Linq;
using System;
using DC.Domain.Models;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionService : BaseService<Permission>, IPermissionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public PermissionService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<PermissionOutput>>> GetTreeTableList(string roleId)
        {
            var res = new ApiResult<List<PermissionOutput>>();

            ISelect<Menu> getSelect() => _fsql.Select<Menu>();

            var childList = await getSelect().Where(m => !string.IsNullOrEmpty(m.ParentId)).ToListAsync();
            var parentList = await getSelect().Where(m => string.IsNullOrEmpty(m.ParentId)).ToListAsync();

            var treeList = new List<PermissionOutput>();
            foreach (var m in parentList)
            {
                treeList.Add(new PermissionOutput()
                {
                    Id = m.Id,
                    Name = m.Name,
                    CreateTime = m.CreateTime,
                    CreateUser = m.CreateUser,
                    ParentId = m.ParentId,
                    Key = m.Id,
                    Checked = GetHasPermission(roleId, m.Id),
                    Children = GetTableChild(childList, roleId, m.Id),
                    Operations = GetOperations(roleId, m.Id)
                });
            }
            res.Data = treeList;
            return res;
        }

        private List<PermissionOutput> GetTableChild(List<Menu> all, string roleId, string id)
        {
            var childer = all.Where(m => m.ParentId == id);
            if (childer.Count() > 0)
            {
                return childer.Select(m => new PermissionOutput()
                {
                    Id = m.Id,
                    Name = m.Name,
                    CreateTime = m.CreateTime,
                    CreateUser = m.CreateUser,
                    ParentId = m.ParentId,
                    Key = m.Id,
                    Checked = GetHasPermission(roleId, m.Id),
                    Children = GetTableChild(all, roleId, m.Id),
                    Operations = GetOperations(roleId, m.Id)
                }).ToList();
            }
            return null;
        }

        private List<OperationListOutPut> GetOperations(string roleId, string menuId)
        {
            return _fsql.Queryable<MenuOperation>()
                        .From<Permission>((mo, p) => mo
                        .LeftJoin(t => t.Id == p.OperationId && p.RoleId == roleId && p.MenuId == menuId))
                        .Where((mo, p) => mo.MenuId == menuId)
                        .ToList((mo, p) => new OperationListOutPut
                        {
                            Label = mo.Name,
                            Value = mo.Id,
                            Checked = p.Id != null
                        });

        }

        private bool GetHasPermission(string roleId, string menuId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return false;
            }
            return _fsql.Select<Permission>()
                        .Where(m => m.MenuId == menuId && m.RoleId == roleId)
                        .Count() > 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysPermissions"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(List<Permission> sysPermissions, string roleId)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Delete<Permission>().Where(m => m.RoleId == roleId).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Insert<Permission>(sysPermissions).WithTransaction(tran).ExecuteAffrowsAsync();
                    _uow.Commit();
                    res++;
                }
            }
            catch (Exception ex)
            {
                res = 0;
                _uow.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _uow.Dispose();
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<NavigationOutput>>> GetNavigationList(string roleId)
        {
            var res = new ApiResult<List<NavigationOutput>>();

            var menuList = await _fsql.Queryable<Menu>()
                                    .From<Permission, MenuOperation>((m, p, o) => m
                                    .LeftJoin(t => t.Id == p.MenuId)
                                    .LeftJoin(t => p.OperationId == o.Id))
                                     .Where((m, p, o) => p.RoleId == roleId)
                                     .OrderBy((m, p, o) => m.Sort)
                                     .Distinct()
                                     .ToListAsync();

            var parent = menuList.Where(m => string.IsNullOrEmpty(m.ParentId));
            var childer = menuList.Where(m => !string.IsNullOrEmpty(m.ParentId)).ToList();

            var allList = await _fsql.Queryable<Menu>()
                                    .From<Permission, MenuOperation>((m, p, o) => m
                                    .LeftJoin(t => t.Id == p.MenuId)
                                    .LeftJoin(t => p.OperationId == o.Id))
                                    .Where((m, p, o) => p.RoleId == roleId && !string.IsNullOrEmpty(o.Code))
                                    .ToListAsync((m, p, o) => new MenuOperation
                                    {
                                        Id = m.Id,
                                        Code = o.Code
                                    });

            var list = new List<NavigationOutput>();
            foreach (var m in parent)
            {
                var buttons = allList.Where(o => o.Id == m.Id).Select(o => o.Code).ToList();

                list.Add(new NavigationOutput()
                {
                    Key = m.Id,
                    PermissionKey = m.Code,
                    Component = m.Component,
                    Path = m.Path,
                    Name = m.Code,
                    Meta = new MetaDetail()
                    {
                        Title = m.Name,
                        Icon = m.Icon,
                        Hidden = false,
                        HiddenHeaderContent = false,
                        HideChildren = false
                    },
                    Children = GetChilder(childer, m.Id, allList),
                    Buttons = buttons
                });
            }

            res.Data = list;
            return res;
        }

        private List<NavigationOutput> GetChilder(List<Menu> childer, string parentId, List<MenuOperation> allList)
        {
            var list = new List<NavigationOutput>();
            foreach (var m in childer.Where(m => m.ParentId == parentId))
            {
                var buttons = allList.Where(o => o.Id == m.Id).Select(o => o.Code).ToList();
                list.Add(new NavigationOutput()
                {
                    Key = m.Id,
                    PermissionKey = m.Code,
                    Component = m.Component,
                    Path = m.Path,
                    Name = m.Code,
                    Meta = new MetaDetail()
                    {
                        Title = m.Name,
                        Icon = m.Icon,
                        Hidden = false,
                        HiddenHeaderContent = false,
                        HideChildren = false
                    },
                    Children = GetChilder(childer, m.Id, allList),
                    Buttons = buttons
                });
            }
            return list;
        }

        public async Task<List<string>> GetOperationPermission(List<string> operationIds)
        {
            return await _fsql.Queryable<MenuOperation>()
                              .From<Permission>((m, p) => m
                              .LeftJoin(t => t.Id == p.OperationId))
                              .Where((m, p) => operationIds.Contains(p.Id))
                              .ToListAsync((m, p) => m.Name);
        }
    }
}
