using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain;
using DC.Service;
using DC.IService;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Output;
using DC.Domain.Models;
using System;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class DCUserService : BaseService<DCUser>, IDCUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public DCUserService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageModel<DCUserOutput>>> QueryAllDCUser(DCUserQuery pageQuery, string myUserId, bool async = true)
        {
            var res = new ApiResult<PageModel<DCUserOutput>>();


            var query = _fsql.Select<DCUser>()
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Name), a => a.Name.Contains(pageQuery.Name) || a.Account.Contains(pageQuery.Name));

            if (pageQuery.RoleId == "商家" || pageQuery.RoleId == "一级推手")
            {
                query.Where(a => a.Id != myUserId && a.ParentId == myUserId);
            }

            // 1. 如果角色是平台 ，获取所有
            // 2. 如果角色是商家，获取他下面所有的用户
            // 3. 如果角色是代理，获取他下面所有的用户



            var totalCount = async ? query.Count() : await query.CountAsync();

            var dbData = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList(a => new DCUserOutput
            {
                Id = a.Id,
                RoleId = a.RoleId,
                Account = a.Account,
                CreateTime = a.CreateTime,
                CreateUser = a.CreateUser,
                LastLoginTime = a.LastLoginTime,
                LoginTotalCount = a.LoginTotalCount,
                Name = a.Name,
                Pwd = a.Pwd,
                ParentId = a.ParentId,
                ParentName = a.Parent.Name,
            }) :
            await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync(a => new DCUserOutput
            {
                Id = a.Id,
                RoleId = a.RoleId,
                Account = a.Account,
                CreateTime = a.CreateTime,
                CreateUser = a.CreateUser,
                LastLoginTime = a.LastLoginTime,
                LoginTotalCount = a.LoginTotalCount,
                Name = a.Name,
                Pwd = a.Pwd,
                ParentId = a.ParentId,
                ParentName = a.Parent.Name,
            });
            res.Data = new PageModel<DCUserOutput>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = dbData
            };
            return res;
        }


        public async Task<ApiResult<DCUserOutput>> GetModelAsync(string id)
        {
            var res = new ApiResult<DCUserOutput>();

            res.Data = await _fsql.Select<DCUser>()
                                  .Where(a => a.Id == id)
                                  .FirstAsync(a => new DCUserOutput
                                  {
                                      Id = a.Id,
                                      Account = a.Account,
                                      LastLoginIp = a.LastLoginIp,
                                      LastLoginTime = a.LastLoginTime,
                                      LoginTotalCount = a.LoginTotalCount,
                                      Name = a.Name,
                                      RoleId = a.RoleId,
                                      ParentName = a.Parent.Name,
                                  });

            return res;
        }

        public async Task<ApiResult<PageModel<DCUserOutput>>> QueryDCUserChilder(DCUserQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<DCUserOutput>>();

            var query = _fsql.Select<DCUser>()
                             .Where(a => a.ParentId == pageQuery.ParentId)
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Name), a => a.Name.Contains(pageQuery.Name))
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.RoleId), a => a.RoleId == pageQuery.RoleId);



            var totalCount = async ? query.Count() : await query.CountAsync();

            var dbData = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList(a => new DCUserOutput
            {
                Id = a.Id,
                RoleId = a.RoleId,
                Account = a.Account,
                CreateTime = a.CreateTime,
                CreateUser = a.CreateUser,
                LastLoginTime = a.LastLoginTime,
                LoginTotalCount = a.LoginTotalCount,
                Name = a.Name,
                Pwd = a.Pwd,
                ParentId = a.ParentId,
                ParentName = a.Parent.Name,
            }) :
            await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync(a => new DCUserOutput
            {
                Id = a.Id,
                RoleId = a.RoleId,
                Account = a.Account,
                CreateTime = a.CreateTime,
                CreateUser = a.CreateUser,
                LastLoginTime = a.LastLoginTime,
                LoginTotalCount = a.LoginTotalCount,
                Name = a.Name,
                Pwd = a.Pwd,
                ParentId = a.ParentId,
                ParentName = a.Parent.Name,
            });
            res.Data = new PageModel<DCUserOutput>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = dbData
            };
            return res;
        }

        public async Task<int> DeleteUserAsync(List<string> ids)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Delete<DCUser>().Where(m => ids.Contains(m.Id)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<UvHistory>().Where(m => ids.Contains(m.DCUserId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<Register>().Where(m => ids.Contains(m.DCUserId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<PvHistory>().Where(m => ids.Contains(m.DCUserId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<Dwz>().Where(m => ids.Contains(m.DCUserId)).WithTransaction(tran).ExecuteAffrowsAsync();
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

        public async Task<int> AddDcUserAsync(DCUser dCUser, List<Dwz> dwzs)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Insert<DCUser>(dCUser).WithTransaction(tran).ExecuteAffrowsAsync();
                    if (dwzs.Count > 0)
                    {
                        await _fsql.Delete<Dwz>().Where(m => m.DCUserId == dCUser.Id).WithTransaction(tran).ExecuteAffrowsAsync();
                        await _fsql.Insert<Dwz>(dwzs).WithTransaction(tran).ExecuteAffrowsAsync();
                    }
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
    }
}
