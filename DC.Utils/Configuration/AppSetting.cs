using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DC.Utils.Configuration
{
    /// <summary>
    /// 配置文件加载
    /// </summary>
    public static class AppSetting
    {
        /// <summary>
        /// 
        /// </summary>
        public static IConfiguration Configuration { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        static AppSetting()
        {
            string Path = "appsettings.json";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!=null)
            {
                //如果你把配置文件 是 根据环境变量来分开了，可以这样写
                Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
            }
            Console.WriteLine(Path);
            Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource
               {
                   Path = Path,
                   Optional = false,
                   ReloadOnChange = true//自动更新
               })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        /// <summary>
        /// 获取配置文件配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static T Get<T>(string fileName = "appsettings", string environmentName = "Development", bool reloadOnChange = false)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory);
            if (!Directory.Exists(filePath))
            {
                Console.WriteLine("没有找到路径：",filePath);
                return default;
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(filePath)
                .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

            if (!string.IsNullOrEmpty(environmentName))
            {
                Console.WriteLine("environmentName不存在：", environmentName);
                builder.AddJsonFile(fileName.ToLower() + "." + environmentName + ".json", true, reloadOnChange);
            }

            var configuration = builder.Build();
            if (configuration == null)
            {
                return default;
            }
            return configuration.Get<T>();
        }

        /// <summary>
        /// 获取配置实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            if (Configuration == null)
                return default;
            return Configuration.Get<T>();
        }
        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string Get(params string[] sections)
        {
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }

                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 多个节点,通过.GetSection("key")["key1"]获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IConfigurationSection GetSection(string key)
        {
            return Configuration?.GetSection(key);
        }
        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }

        /// <summary>
        /// 获取列表节点
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IConfigurationSection> GetChildren()
        {
            return Configuration?.GetChildren();
        }
    }
}
