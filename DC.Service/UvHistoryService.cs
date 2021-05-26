using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain;
using DC.Service;
using DC.IService;
using DC.Domain.Global;
using DC.Domain.Input;
using System;
using DC.Domain.Output;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UvHistoryService : BaseService<UvHistory>, IUvHistoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public UvHistoryService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        public async Task<int> AddModelrAsync(Register register, UvHistory uvHistory, bool async = true)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    if (register.Id != null)
                    {
                        await _fsql.Insert<Register>(register).WithTransaction(tran).ExecuteAffrowsAsync();
                    }
                    if (uvHistory.Id != null)
                    {
                        await _fsql.Insert<UvHistory>(uvHistory).WithTransaction(tran).ExecuteAffrowsAsync();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageModel<UvHistorycpaCps>>> GetPagesAsync(UvHistoryQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<UvHistorycpaCps>>();


            var query = _fsql.Queryable<Register>()
                                   .From<Product, DCUser>((r, p, d) => r
                                   .LeftJoin(t => t.ProductId == p.Id)
                                   .LeftJoin(t => t.DCUserId == d.Id))
                                   .WhereIf(!string.IsNullOrEmpty(pageQuery.Key), (r, p, d) => r.Name.Contains(pageQuery.Key) || p.Name.Contains(pageQuery.Key))
                                   .WhereIf(!string.IsNullOrEmpty(pageQuery.Status), (r, p, d) => r.Status == pageQuery.Status)
                                   .WhereIf(pageQuery.Time != null, (r, p, d) => r.CreateTime.Date == pageQuery.Time).OrderByDescending((r, p, d) => r.CreateTime);

            if (pageQuery.LinkModel.ToLower() == "cps")
            {
                query = query.Where((r, p, d) => p.LinkModel.ToLower() == "cps");
            }
            else
            {
                query = query.Where((r, p, d) => p.LinkModel.ToLower().Contains("cpa"));
            }

            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<UvHistorycpaCps>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList((u, p, d) => new UvHistorycpaCps
                {
                    Id = u.Id,
                    Ip = u.Ip,
                    Name = u.Name,
                    Phone = u.Tel,
                    SystemType = u.SystemType,
                    Status = u.Status,
                    DCUserDevName = d.Name,
                    ProductName = p.Name,
                    CreateTime = u.CreateTime.ToString("yyyy-MM-dd")
                }) : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync((u, p, d) => new UvHistorycpaCps
                {
                    Id = u.Id,
                    Ip = u.Ip,
                    Name = u.Name,
                    Phone = u.Tel,
                    SystemType = u.SystemType,
                    Status = u.Status,
                    DCUserDevName = d.Name,
                    ProductName = p.Name,
                    CreateTime = u.CreateTime.ToString("yyyy-MM-dd")
                })
            };
            return res;
        }

        public async Task<List<ReportLine>> GetReportByToday()
        {
            var sql = @"SELECT a.hour hour, ifnull(b.count, 0) totalCount FROM (
                    SELECT 0 hour UNION ALL SELECT 1 hour UNION ALL SELECT 2 hour UNION ALL SELECT 3 hour UNION ALL SELECT 4 hour UNION ALL SELECT 5 hour UNION ALL SELECT 6 hour UNION ALL SELECT 7 hour UNION ALL SELECT 8 hour UNION ALL SELECT 9 hour UNION ALL SELECT 10 hour UNION ALL SELECT 11 hour UNION ALL SELECT 12 hour
                    UNION ALL SELECT 13 hour UNION ALL SELECT 14 hour UNION ALL SELECT 15 hour UNION ALL SELECT 16 hour UNION ALL SELECT 17 hour UNION ALL SELECT 18 hour UNION ALL SELECT 19 hour UNION ALL SELECT 20 hour UNION ALL SELECT 21 hour UNION ALL SELECT 22 hour UNION ALL SELECT 23 hour
                                  ) a LEFT JOIN
                      (
                        SELECT
                          hour(createtime)  hour,
                          count(createtime) count
                        FROM uvhistory
                        WHERE date_format(createtime, '%Y-%m-%d') = date_format(now(), '%Y-%m-%d')
                        GROUP BY date_format(createtime, '%Y%m%d-%H'), hour
                      ) b
                      ON a.hour = b.hour

                         WHERE a.hour <= HOUR(NOW())
                    ORDER BY hour";

            return await _fsql.Ado.QueryAsync<ReportLine>(sql);
        }

        public async Task<List<ReportLine>> GetReportDevByToday(string userId)
        {
            var sql = @"SELECT a.hour hour, ifnull(b.count, 0) totalCount FROM (
                    SELECT 0 hour UNION ALL SELECT 1 hour UNION ALL SELECT 2 hour UNION ALL SELECT 3 hour UNION ALL SELECT 4 hour UNION ALL SELECT 5 hour UNION ALL SELECT 6 hour UNION ALL SELECT 7 hour UNION ALL SELECT 8 hour UNION ALL SELECT 9 hour UNION ALL SELECT 10 hour UNION ALL SELECT 11 hour UNION ALL SELECT 12 hour
                    UNION ALL SELECT 13 hour UNION ALL SELECT 14 hour UNION ALL SELECT 15 hour UNION ALL SELECT 16 hour UNION ALL SELECT 17 hour UNION ALL SELECT 18 hour UNION ALL SELECT 19 hour UNION ALL SELECT 20 hour UNION ALL SELECT 21 hour UNION ALL SELECT 22 hour UNION ALL SELECT 23 hour
                                  ) a LEFT JOIN
                      (
                        SELECT
                          hour(createtime)  hour,
                          count(createtime) count
                        FROM uvhistory
                        WHERE  dcuserid = '@dcuserid' and date_format(createtime, '%Y-%m-%d') = date_format(now(), '%Y-%m-%d') 
                        GROUP BY date_format(createtime, '%Y%m%d-%H'), hour
                      ) b
                      ON a.hour = b.hour

                         WHERE a.hour <= HOUR(NOW())
                    ORDER BY hour";

            return await _fsql.Ado.QueryAsync<ReportLine>(sql, new { dcuserid = userId });
        }

        public async Task<long> GetTotalCountAsync(ReportQuery reportQuery, string productId, string systemType = null)
        {
            return await _fsql.Select<UvHistory>().Where(m => m.ProductId == productId)
                              .WhereIf(systemType != null, m => m.SystemType.ToLower().Contains(systemType))
                              .WhereIf(reportQuery.Date != null, m => m.CreateTime.Date == reportQuery.Date)
                              .WhereIf(reportQuery.Date == null, m => m.CreateTime.Date == DateTime.Now.Date)
                              .CountAsync();
        }

    }

}