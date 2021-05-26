using DC.Domain.Global;
using DC.IService;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DC.Service
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IFreeSql _fsql;
        /// <summary>
        /// 
        /// </summary>
        public readonly IUnitOfWork _uow;
        /// <summary>
        /// 
        /// </summary>
        public BaseService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public BaseService(IFreeSql freeSql, IUnitOfWork uow)
        {
            _fsql = freeSql;
            _uow = uow;
        }
        #region 增加

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(T t, bool async = true)
        {
            return async ? _fsql.Insert<T>(t).ExecuteAffrows() : await _fsql.Insert<T>(t).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<int> AddListAsync(List<T> t, bool async = true)
        {
            return async ? _fsql.Insert<T>(t).ExecuteAffrows() : await _fsql.Insert<T>(t).ExecuteAffrowsAsync();
        }

        #endregion

        #region 删除

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string sql, bool async = true)
        {
            return async ? _fsql.Ado.ExecuteNonQuery(sql) : await _fsql.Ado.ExecuteNonQueryAsync(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where, bool async = true)
        {
            return async ? _fsql.Delete<T>().Where(where).ExecuteAffrows() : await _fsql.Delete<T>().Where(where).ExecuteAffrowsAsync();
        }
        #endregion

        #region 修改

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T t, bool async = true)
        {
            return async ? _fsql.Update<T>().SetSource(t).ExecuteAffrows() : (await _fsql.Update<T>().SetSource(t).ExecuteAffrowsAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<T> t, bool async = true)
        {
            return async ? _fsql.Update<T>().SetSource(t).ExecuteAffrows() : (await _fsql.Update<T>().SetSource(t).ExecuteAffrowsAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        public async Task<int> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where, bool async = true)
        {
            return async ? _fsql.Update<T>().Set(columns).Where(where).ExecuteAffrows() : await _fsql.Update<T>().Set(columns).Where(where).ExecuteAffrowsAsync();
        }
        #endregion

        #region 获取

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="orderEnum"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, string orderEnum = "ASC", bool async = true)
        {
            var query = _fsql.Select<T>().Where(where);
            if (order != null)
            {
                if (orderEnum == "ASC")
                {
                    query = query.OrderBy(order);
                }
                else
                {
                    query = query.OrderByDescending(order);
                }
            }
            return async ? query.ToList<T>() : await query.ToListAsync<T>();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, int tack, Expression<Func<T, object>> order = null, string orderEnum = "ASC", bool async = true)
        {
            var query = _fsql.Select<T>().Where(where);
            if (order != null)
            {
                if (orderEnum == "ASC")
                {
                    query = query.OrderBy(order);
                }
                else
                {
                    query = query.OrderByDescending(order);
                }
            }
            return async ? query.Take(tack).ToList<T>() : await query.Take(tack).ToListAsync<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(bool async = true)
        {
            return async ? _fsql.Select<T>().ToList<T>() : await _fsql.Select<T>().ToListAsync<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<List<string>> GetSelectAsync(Expression<Func<T, bool>> where, Expression<Func<T, string>> select, bool async = true)
        {
            var query = _fsql.Select<T>().Where(where);
            return async ? query.ToList<string>(select) : await query.ToListAsync<string>(select);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(string sql, bool async = true)
        {
            return async ? _fsql.Select<T>().WithSql(sql).First<T>() : await _fsql.Select<T>().WithSql(sql).FirstAsync<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(Expression<Func<T, bool>> where, bool async = true)
        {
            return async ? _fsql.Select<T>().Where(where).First() : await _fsql.Select<T>().Where(where).FirstAsync<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageParm"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        public async Task<PageModel<T>> GetPagesAsync(PageQuery pageParm, bool async = true)
        {
            var query = _fsql.Select<T>();
            var totalCount = async ? query.Count() : await query.CountAsync();
            var totalPages = totalCount != 0 ? (totalCount % pageParm.Current) == 0 ? (totalCount / pageParm.Current) : (totalCount / pageParm.Current) + 1 : 0;

            var pageList = new PageModel<T>()
            {
                PageNo = pageParm.Current,
                PageSize = pageParm.PageSize,
                TotalCount = totalCount,
                TotalPage = totalPages,
                Data = async ? query.Page(pageParm.Current, pageParm.PageSize).ToList<T>() : await query.Page(pageParm.Current, pageParm.PageSize).ToListAsync<T>()
            };
            return pageList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageParm"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="orderEnum"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<PageModel<T>> GetPagesAsync(PageQuery pageParm, Expression<Func<T, bool>> where, Expression<Func<T, object>> order = null, string orderEnum = "ASC", bool async = true)
        {
            var query = _fsql.Select<T>().Where(where);
            var totalCount = async ? query.Count() : await query.CountAsync();
            var totalPages = totalCount != 0 ? (totalCount % pageParm.Current) == 0 ? (totalCount / pageParm.Current) : (totalCount / pageParm.Current) + 1 : 0;

            var pageList = new PageModel<T>()
            {
                PageNo = pageParm.Current,
                PageSize = pageParm.PageSize,
                TotalCount = totalCount,
                TotalPage = totalPages
            };
            if (order != null)
            {
                if (orderEnum == "ASC")
                {
                    query = query.OrderBy(order);
                }
                else
                {
                    query = query.OrderByDescending(order);
                }
            }
            pageList.Data = async ? query.Page(pageParm.Current, pageParm.PageSize).ToList<T>() : await query.Page(pageParm.Current, pageParm.PageSize).ToListAsync<T>();

            return pageList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> where, bool async = true)
        {
            return async ? _fsql.Select<T>().Where(where).Count() > 0 ? true : false : await _fsql.Select<T>().Where(where).CountAsync() > 0 ? true : false;
        }

        public async Task<long> GetTotalCountAsync(Expression<Func<T, bool>> where, bool async = true)
        {
            var query = _fsql.Select<T>().Where(where);

            return async ? query.Count() : await query.CountAsync();
        }

        public async Task<T> GetMaxModelAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, bool async = true)
        {
            var query = _fsql.Select<T>().Where(where).OrderByDescending(order);

            return async ? query.First(): await query.FirstAsync();
        }
        #endregion
    }
}
