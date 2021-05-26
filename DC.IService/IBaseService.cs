using DC.Domain.Global;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DC.IService
{
    /// <summary>
    /// 定义基本服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T> where T : class
    {
        #region 添加操作
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<int> AddAsync(T t, bool async = true);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <returns></returns>
        Task<int> AddListAsync(List<T> t, bool async = true);

        #endregion

        #region 查询操作
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="orderEnum"> 默认 ASC</param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order = null, string orderEnum = "ASC", bool async = true);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<List<string>> GetSelectAsync(Expression<Func<T, bool>> where, Expression<Func<T, string>> select, bool async = true);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, int tack, Expression<Func<T, object>> order = null, string orderEnum = "ASC", bool async = true);
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync(bool async = true);

        /// <summary>
        /// 获得列表——分页
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<PageModel<T>> GetPagesAsync(PageQuery pageQuery, bool async = true);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageQuery">分页参数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序值</param>
        /// <param name="orderEnum">排序方式OrderByType 默认 ASC</param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<PageModel<T>> GetPagesAsync(PageQuery pageQuery, Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order = null, string orderEnum = "ASC", bool async = true);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<T> GetModelAsync(string sql, bool async = true);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<T> GetModelAsync(Expression<Func<T, bool>> where, bool async = true);

        /// <summary>
        /// 检查是否重复
        /// </summary>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> where, bool async = true);


        /// <summary>
        /// 查询数据量
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<long> GetTotalCountAsync(Expression<Func<T, bool>> where,bool async = true);

        /// <summary>
        /// 获取排序倒叙的第一条数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<T> GetMaxModelAsync(Expression<Func<T, bool>> where, Expression<Func<T,object>> order, bool async = true);
        #endregion

        #region 修改操作
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T t, bool async = true);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="Async"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<T> t, bool Async = true);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, T>> columns,
            Expression<Func<T, bool>> where, bool async = true);
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="sql">删除的sql语句</param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(string sql, bool async = true);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> where, bool async = true);

        #endregion

    }
}
