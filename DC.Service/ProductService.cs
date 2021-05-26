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
using DC.Domain.Models;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductService : BaseService<Product>, IProductService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public ProductService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        public async Task<int> DeleteProdAsync(List<string> ids, bool async = true)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Delete<Product>().Where(m => ids.Contains(m.Id)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<UvHistory>().Where(m => ids.Contains(m.ProductId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<Register>().Where(m => ids.Contains(m.ProductId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<PvHistory>().Where(m => ids.Contains(m.ProductId)).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Delete<Dwz>().Where(m => ids.Contains(m.ProductId)).WithTransaction(tran).ExecuteAffrowsAsync();
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

        public async Task<ApiResult<PageModel<Product>>> GetPagesAsync(ProductQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<Product>>();

            var query = _fsql.Select<Product>()
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Name), m => m.Name.Contains(pageQuery.Name))
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Account), m => m.CreateUser == pageQuery.Account)
                             .OrderByDescending(b => b.Sort);


            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<Product>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList() : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync()
            };
            return res;
        }

        public async Task<PageModel<ProductPageDevOutput>> GetPagesByDevAsync(ProductQuery pageQuery,string userId, bool async = true)
        { 
            var query = _fsql.Select<Product, Dwz>()
                             .LeftJoin((a, b) => a.Id == b.ProductId)
                             .Where((a, b) => a.AuditStatus == "上架中" && b.DCUserId == userId)
                             .WhereIf(!string.IsNullOrEmpty(pageQuery.Name), (a, b) => a.Name.Contains(pageQuery.Name))
                             .OrderBy((a, b) => a.Sort);

            var totalCount = async ? query.Count() : await query.CountAsync();

            return new PageModel<ProductPageDevOutput>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList((a, b) => new ProductPageDevOutput()
                {
                    Id = a.Id,
                    AliasName = a.AliasName,
                    Name = a.Name,
                    ExtendMoney = a.ExtendMoney,
                    LinkModel = a.LinkModel,
                    Logo = a.Logo,
                    Ecode = b.EncrypCodeParams
                }) : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync((a, b) => new ProductPageDevOutput()
                {
                    Id = a.Id,
                    AliasName = a.AliasName,
                    Name = a.Name,
                    ExtendMoney = a.ExtendMoney,
                    LinkModel = a.LinkModel,
                    Logo = a.Logo,
                    Ecode = b.EncrypCodeParams
                })
            }; 
        }

        public async Task<ApiResult<PageModel<ProductH5Output>>> QueryPagesProductH5(ProductQuery pageQuery, bool async = true)
        {
            var res = new ApiResult<PageModel<ProductH5Output>>();

            var query = _fsql.Select<Product>()
                             .Where(m => m.H5)
                             .OrderBy(m => m.Sort);

            var totalCount = async ? query.Count() : await query.CountAsync();

            res.Data = new PageModel<ProductH5Output>()
            {
                PageNo = pageQuery.Current,
                PageSize = pageQuery.PageSize,
                TotalCount = totalCount,
                TotalPage = (totalCount + pageQuery.PageSize - 1) / pageQuery.PageSize,
                Data = async ? query.Page(pageQuery.Current, pageQuery.PageSize).ToList(m => new ProductH5Output
                {
                    Id = m.Id,
                    Name = m.Name,
                    Logo = m.Logo,
                    Type = m.ProductType,
                    Tag = m.Tag,
                    Edu = m.ExtendMoney,
                    Url = m.Url,

                }) : await query.Page(pageQuery.Current, pageQuery.PageSize).ToListAsync(m => new ProductH5Output
                {
                    Id = m.Id,
                    Name = m.Name,
                    Logo = m.Logo,
                    Type = m.ProductType,
                    Tag = m.Tag,
                    Edu = m.ExtendMoney,
                    Url = m.Url
                })
            };
            return res;
        }

        public async Task<ApiResult<List<Product>>> QueryProductListAsync(string account, bool async = true)
        {
            var res = new ApiResult<List<Product>>();

            res.Data = await _fsql.Select<Product>().Where(m => m.CreateUser == account).ToListAsync();

            return res;
        }

        public async Task<int> SortProductAsync(Product input, Product product)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Update<Product>(input).Set(m => m.Sort, input.Sort).WithTransaction(tran).ExecuteAffrowsAsync();
                    await _fsql.Update<Product>(product).Set(m => m.Sort, product.Sort).WithTransaction(tran).ExecuteAffrowsAsync();
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

        public async Task<int> UpdateProduAsync(Product product, List<Dwz> dwzs)
        {
            var res = 0;
            try
            {
                using (var tran = _uow.GetOrBeginTransaction())
                {
                    await _fsql.Update<Product>(product).Set(m => m.AuditStatus, product.AuditStatus).WithTransaction(tran).ExecuteAffrowsAsync();
                    if (dwzs.Count > 0)
                    {
                        await _fsql.Delete<Dwz>().Where(m => m.ProductId == product.Id).WithTransaction(tran).ExecuteAffrowsAsync();
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
