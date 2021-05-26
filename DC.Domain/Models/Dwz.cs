using FreeSql.DataAnnotations;
using System;

namespace DC.Domain.Models
{
    /// <summary>
    /// 短网址
    /// </summary>
    [Table(Name = "Dwz")]
    public class Dwz
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 推手ID
        /// </summary>
        public string DCUserId { get; set; }

        /// <summary>
        /// 加密短码
        /// </summary>
        public string EncrypCodeParams { get; set; }

        /// <summary>
        /// 过期时间（为null时是永久）
        /// </summary>
        public DateTime? ExpiryTime { get; set; }
    }
}
