using FreeSql.DataAnnotations;
using System;

namespace DC.Domain.Models
{
    /// <summary>
    /// Api请求日志模型
    /// </summary>
    [Table(Name = "ApiLog")]
    public class ApiLog
    {
        /// <summary>
        /// 主键 唯一编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 操作IP地址
        /// </summary>
        [Column(DbType = "varchar(500)")]
        public string RemoteIp { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public double ElapsedTime { get; set; }

        /// <summary>
        /// 方法类
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [Column(DbType = "text")]
        public string RequestParams { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
