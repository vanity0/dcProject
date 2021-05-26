using FreeSql.DataAnnotations;
using System;

namespace DC.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table(Name = "DCUser")]
    public class DCUser
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
        /// 手机号（账号）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 所属角色：平台，商家，一级推手，二级推手
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 预付
        /// </summary>
        public double Prepay { get; set; }

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

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录IP地址
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 登录总次数
        /// </summary>
        public int LoginTotalCount { get; set; }

        public virtual DCUser Parent{ get; set; }
    }
}
