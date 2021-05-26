using FreeSql.DataAnnotations;
using System;

namespace DC.Domain
{
    /// <summary>
    /// uv（点击量）
    /// </summary>
    [Table(Name = "UvHistory")]
    public class UvHistory
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
        ///  系统（Andrord；iOS）  
        /// </summary>
        public string SystemType { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
