using System.Collections.Generic;
using System.Threading.Tasks;
using DC.Domain;
using DC.Domain.Models;
using DC.Domain.Output;

namespace DC.IService
{
    /// <summary>
    /// 短网址接口
    /// </summary>
    public interface IDwzService : IBaseService<Dwz>
    {
        /// <summary>
        /// 获取所有地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<DwzOutput>> GetAllDwzUrlAsync(string userId);
    }
}
