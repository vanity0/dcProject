using FreeSql.DataAnnotations;
using System;

namespace DC.Domain
{
    /// <summary>
    /// A类产品
    /// </summary>
    [Table(Name = "Product")]
    public class Product
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品Logo
        /// </summary>
        [Column(DbType = "mediumtext")]
        public string Logo { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 系列名称
        /// </summary>
        public string AliasName { get; set; } 

        /// <summary>
        /// 描述
        /// </summary>
        [Column(DbType = "varchar(1024)")]
        public string Descption { get; set; }

        /// <summary>
        /// 标签：利息低，审批快，门槛低，额度高，可分期，还款灵活
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 上架H5
        /// </summary>
        public bool H5 { get; set; }

        /// <summary>
        /// 产品状态（"待申请", "审核中","审核通过","拒绝通过", "上架中", "下架中"）
        /// </summary>
        public string AuditStatus { get; set; }

        /// <summary>
        /// 产品分类（小额、大额、权益、助贷、贷超） 
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 接入模式,甲方付费方式：以CPA、CPS来结算价格
        /// </summary>
        public string LinkModel { get; set; }

        /// <summary>
        /// 推广价格,推手端推广价格
        /// </summary>
        public double ExtendMoney { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long Sort { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
