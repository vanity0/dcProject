using FreeSql;
using System.Threading.Tasks;
using System.Collections.Generic;
using DC.Domain;
using DC.Service;
using DC.IService;
using DC.Domain.Global;
using DC.Domain.Input;
using DC.Domain.Output;
using System;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterService : BaseService<Register>, IRegisterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public RegisterService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        public async Task<ApiResult<PageModel<RegisterOutput>>> GetPagesAsync(RegisterQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<RegisterOutput>>();

            var query = _fsql.Select<Register, DCUser, Product>()
                             .LeftJoin((a, b, c) => a.DCUserId == b.Id)
                             .LeftJoin((a, b, c) => a.ProductId == c.Id)
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Name), (a, b, c) => a.Name.Contains(pageQuery.Name) || a.Tel.Contains(pageQuery.Name))
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.ProductId), (a, b, c) => a.ProductId == pageQuery.ProductId)
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.DCUserId), (a, b, c) => a.DCUserId == pageQuery.DCUserId)
                             .WhereIf(pageQuery.StartDate != null && pageQuery.EndDate != null, (a, b, c) => a.CreateTime >= pageQuery.StartDate && a.CreateTime <= pageQuery.EndDate)
                             ;


            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<RegisterOutput>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).OrderByDescending((a, b, c) => a.CreateTime).ToList((a, b, c) => new RegisterOutput
                {
                    Id = a.Id,
                    Name = a.Name,
                    Tel = a.Tel,
                    Status = a.Status,
                    CreateTime = a.CreateTime,
                    ProductName = c.Name,
                    UserName = b.Name,
                    Ip = a.Ip,
                    SystemType = a.SystemType,
                }) : await query.Page(pageQuery.Current, pageQuery.PageSize).OrderByDescending((a, b, c) => a.CreateTime).ToListAsync((a, b, c) => new RegisterOutput
                {
                    Id = a.Id,
                    Name = a.Name,
                    Tel = a.Tel,
                    Status = a.Status,
                    CreateTime = a.CreateTime,
                    ProductName = c.Name,
                    UserName = b.Name,
                    Ip = a.Ip,
                    SystemType = a.SystemType,
                })
            };
            return res;
        }

        public async Task<ApiResult<RegisterOutput>> GetModelByIdAsync(string id, bool async)
        {
            var res = new ApiResult<RegisterOutput>();
            res.Data = await _fsql.Select<Register, DCUser, Product>()
                            .LeftJoin((a, b, c) => a.DCUserId == b.Id)
                            .LeftJoin((a, b, c) => a.ProductId == c.Id)
                            .Where((a, b, c) => a.Id == id)
                            .FirstAsync((a, b, c) => new RegisterOutput
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Tel = a.Tel,
                                Status = a.Status,
                                CreateTime = a.CreateTime,
                                AuditTime = a.AuditTime,
                                AuditUser = a.AuditUser,
                                ProductName = c.Name,
                                UserName = b.Name,
                                Ip = a.Ip,
                                SystemType = a.SystemType,
                            });

            return res;
        }

        public async Task<long> GetTotalCountAsync(ReportQuery reportQuery, string productId)
        {
            return await _fsql.Select<Register>().Where(m => m.ProductId == productId && m.Status == "待更新")
                              .WhereIf(reportQuery.Date != null, m => m.CreateTime.Date == reportQuery.Date)
                              .WhereIf(reportQuery.Date == null, m => m.CreateTime.Date == DateTime.Now.Date)
                              .CountAsync();
        }

    }
}
