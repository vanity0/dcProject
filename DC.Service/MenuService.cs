using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain;
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
    public class MenuService : BaseService<Menu>, IMenuService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public MenuService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        public async Task<ApiResult<List<MenuOutput>>> GetTreeTablePagesAsync(MenuQuery query, bool async = true)
        {
            var res = new ApiResult<List<MenuOutput>>();

            ISelect<Menu> getSelect() => _fsql.Select<Menu>();


            var childList = getSelect().Where(m => !string.IsNullOrEmpty(m.ParentId)).ToList();
            var parentQuery = getSelect().Where(m => string.IsNullOrEmpty(m.ParentId));

            var totalCount = async ? parentQuery.Count() : await parentQuery.CountAsync();
            var parentList = async ? parentQuery.OrderBy(m => m.Sort).ToList() :
                               await parentQuery.OrderBy(m => m.Sort).ToListAsync();


            var treeList = new List<MenuOutput>();
            foreach (var m in parentList)
            {
                treeList.Add(new MenuOutput()
                {
                    Id = m.Id,
                    Name = m.Name,
                    CreateTime = m.CreateTime,
                    CreateUser = m.CreateUser,
                    Icon = m.Icon,
                    MenuType = m.MenuType,
                    ParentId = m.ParentId,
                    Code = m.Code,
                    Path = m.Path,
                    Sort = m.Sort,
                    Component = m.Component,
                    Key = m.Id,
                    Children = GetTableChild(childList, m.Id)
                });
            }

            res.Data = treeList;
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="all"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<MenuOutput> GetTableChild(List<Menu> all, string id)
        {
            var childer = all.Where(m => m.ParentId == id);
            if (childer.Count() > 0)
            {
                return childer.Select(m => new MenuOutput()
                {
                    Id = m.Id,
                    Name = m.Name,
                    CreateTime = m.CreateTime,
                    CreateUser = m.CreateUser,
                    Icon = m.Icon,
                    MenuType = m.MenuType,
                    ParentId = m.ParentId,
                    Code = m.Code,
                    Path = m.Path,
                    Sort = m.Sort,
                    Component = m.Component,
                    Key = m.Id,
                    Children = GetTableChild(all, m.Id)
                }).ToList();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<TreeSelectOutPut>>> QueryMenuTreeSelect(string menuType)
        {
            var res = new ApiResult<List<TreeSelectOutPut>>();

            res.Data = await _fsql.Queryable<Menu>()
                            .WhereIf(menuType != null, m => m.MenuType == menuType)
                            .ToListAsync(m => new TreeSelectOutPut()
                            {
                                Id = m.Id,
                                PId = m.ParentId,
                                Title = m.Name,
                                Value = m.Id,
                                IsLeaf = !string.IsNullOrEmpty(m.ParentId) ? true : false
                            });
            return res;
        }

        public async Task<int> AddAsync(MenuInput menu, List<MenuOperation> list)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Insert<MenuOperation>(list).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Insert<Menu>(menu).WithTransaction(tran).ExecuteAffrowsAsync();
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

        public async Task<int> UpdateAsync(MenuInput menu, List<MenuOperation> updateList, List<MenuOperation> newList)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Update<MenuOperation>()
                               .SetSource(updateList)
                               .UpdateColumns(a => new { a.Name, a.Code })
                               .WithTransaction(tran)
                               .ExecuteAffrowsAsync();

                    await _fsql.Insert<MenuOperation>(newList).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Update<Menu>().SetSource(menu).WithTransaction(tran).ExecuteAffrowsAsync();
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteMenuAsync(List<string> ids)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Delete<MenuOperation>().Where(m => ids.Contains(m.MenuId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<Menu>().Where(m => ids.Contains(m.Id)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<Permission>().Where(m => ids.Contains(m.Id)).WithTransaction(tran).ExecuteAffrowsAsync();
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

        public async Task<MenuInput> GetModelAsync(string id)
        {
            var model = await _fsql.Select<Menu>()
                             .Where(m => m.Id == id)
                             .FirstAsync<MenuInput>();

            if (model != null)
            {
                model.Operations = await _fsql.Select<MenuOperation>()
                                                .Where(m => m.MenuId == model.Id)
                                                .ToListAsync();
            }

            return model;
        }

    }
}
