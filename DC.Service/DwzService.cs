using FreeSql;
using System.Threading.Tasks;
using DC.IService;
using DC.Domain.Models;
using System.Collections.Generic;
using DC.Domain.Output;
using DC.Domain;

namespace DC.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class DwzService : BaseService<Dwz>, IDwzService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="uow"></param>
        public DwzService(IFreeSql freeSql, IUnitOfWork uow) : base(freeSql, uow)
        {
        }

        public async Task<List<DwzOutput>> GetAllDwzUrlAsync(string userId)
        {
            try
            {
                return await _fsql.Select<Dwz, Product>()
                                    .LeftJoin((a, b) => a.ProductId == b.Id)
                                    .Where((a, b) => a.DCUserId == userId && b.AuditStatus == "上架中")
                                    .ToListAsync((a, b) => new DwzOutput()
                                    {
                                        Url = a.EncrypCodeParams,
                                        ProdName = b.Name,
                                        AliasName =b.AliasName
                                    });
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("GetAllDwzUrlAsync:" + ex.Message);
            }
            return new List<DwzOutput>();
        }
    }
}
