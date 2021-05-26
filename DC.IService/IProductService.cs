using System.Threading.Tasks;
using DC.Domain.Global;
using DC.IService;
using DC.Domain;
using DC.Domain.Input;
using System.Collections.Generic;
using DC.Domain.Output;
using DC.Domain.Models;

namespace DC.IService
{
    public interface IProductService : IBaseService<Product>
    {
        Task<int> DeleteProdAsync(List<string> ids, bool async = true);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>

        Task<ApiResult<PageModel<Product>>> GetPagesAsync(ProductQuery query, bool async = true);

        /// <summary>
        /// 推手产品列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<PageModel<ProductPageDevOutput>> GetPagesByDevAsync(ProductQuery query, string userId, bool async = true);

        /// <summary>
        /// H5产品
        /// </summary>
        /// <param name="query"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<PageModel<ProductH5Output>>> QueryPagesProductH5(ProductQuery pageQuery, bool async = true);

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        Task<ApiResult<List<Product>>> QueryProductListAsync(string account, bool async = true);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<int> SortProductAsync(Product input, Product product);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="product"></param>
        /// <param name="dwzs"></param>
        /// <returns></returns>
        Task<int> UpdateProduAsync(Product product, List<Dwz> dwzs);
    }
}
