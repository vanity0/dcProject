using FreeSql.DataAnnotations;
using System;

namespace DC.Domain
{
    /// <summary>
    /// 申请数据
    /// </summary>
    [Table(Name = "Register")]
    public class Register
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 手机型号
        /// </summary>
        public string SystemType { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 推手ID
        /// </summary>
        public string DCUserId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 申请状态（待更新（注册后就是该状态），审核已通过（由商家/平台审核改变），审核未通过）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
    }
}
