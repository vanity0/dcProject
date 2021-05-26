
using FreeSql;

namespace DC.Api.Models
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {

        /// <summary>
        /// Api地址，默认 http://*:2020
        /// </summary>
        public string Url { get; set; } = "http://*:2020";

        /// <summary>
        /// IP限制
        /// </summary>
        public IpConfig IpConfig { get; set; }

        /// <summary>
        /// Https设置
        /// </summary>
        public HttpsConfig HttpsConfig { get; set; }

        /// <summary>
        /// 数据库设置
        /// </summary>
        public DbConfig DbConfig { get; set; }

        /// <summary>
        /// 登录设置
        /// </summary>
        public LoginConfig LoginConfig { get; set; }

        /// <summary>
        /// JWT
        /// </summary>
        public JwtConfig JwtConfig { get; set; }
    }

    /// <summary>
    /// IP限制
    /// </summary>
    public class IpConfig
    {
        /// <summary>
        /// 是否启动IP限制 默认false
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 限制的城市
        /// </summary>
        public string DenyArea { get; set; }

    }

    /// <summary>
    /// Https设置
    /// </summary>
    public class HttpsConfig
    {
        /// <summary>
        /// 是否启用https跳转
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 证书路径
        /// </summary>
        public string CertPath { get; set; }

        /// <summary>
        /// 证书密钥
        /// </summary>
        public string Securitykey { get; set; }
    }

    /// <summary>
    /// 缓存设置
    /// </summary>
    public class CacheConfig
    {

        /// <summary>
        /// 缓存类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Redis配置链接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }

    }

    /// <summary>
    /// 数据库设置
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 是否创建数据库
        /// </summary>
        public bool IsCreateDb { get; set; }

        /// <summary>
        /// 创建数据库的连接字符串脚本
        /// </summary>
        public string CreateDbConn { get; set; }

        /// <summary>
        /// 创建数据库的脚本
        /// </summary>
        public string CreateDbSql { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DbType { get; set; }

        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 监听所有操作
        /// </summary>
        public bool MonitorCommand { get; set; }

        /// <summary>
        /// 监听Curd操作
        /// </summary>
        public bool Curd { get; set; }

        /// <summary>
        /// 是否自动迁徙实体到数据库
        /// </summary>
        public bool SyncStructure { get; set; }

    }

    /// <summary>
    /// 登录设置
    /// </summary>
    public class LoginConfig
    {
        /// <summary>
        /// Session 关闭浏览器需要重新登录，  Cookie  需要读取下面的过期时间
        /// </summary>
        public string SaveType { get; set; }

        /// <summary>
        /// 过期小时
        /// </summary>
        public int ExpiresHours { get; set; } = 2;

        /// <summary>
        /// 超过设定值次数账号密码输入错误，则延时登录
        /// </summary>
        public int Count { get; set; } = 3;

        /// <summary>
        /// 超过次数，延时分钟数
        /// </summary>
        public int DelayMinute { get; set; } = 5;
    }


    /// <summary>
    /// jwt令牌
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 有效期(分钟)
        /// </summary>
        public double Expires { get; set; }
    }
}
