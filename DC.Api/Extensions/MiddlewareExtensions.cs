using DC.Api.Models;
using FreeSql;
using FreeSql.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DC.Api.Extensions
{
    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 添加ORM以及数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        /// <param name="appConfig"></param>
        public async static void AddFreeSql(this IServiceCollection services, IHostEnvironment env,
            AppConfig appConfig)
        {
            try
            {
                //创建数据库
                if (appConfig.DbConfig.IsCreateDb)
                {
                    await CreateDatabase(appConfig.DbConfig);
                }

                var freeSqlBuilder = new FreeSqlBuilder()
                        .UseNameConvert(NameConvertType.ToLower)
                        .UseConnectionString(appConfig.DbConfig.DbType, appConfig.DbConfig.ConnectionString)
                        .UseAutoSyncStructure(appConfig.DbConfig.SyncStructure) //自动迁移实体的结构到数据库
                        .UseLazyLoading(false)
                        .UseNoneCommandParameter(true);

                var fsql = freeSqlBuilder.Build();
                services.AddScoped<IUnitOfWork>(sp => fsql.CreateUnitOfWork());
                services.AddSingleton(fsql);

                #region 监听所有命令
                if (appConfig.DbConfig.MonitorCommand)
                {
                    freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                    {
                        Console.WriteLine($"{cmd.CommandText}\n{traceLog}\r\n");
                    });
                }
                #endregion

                #region 监听Curd操作
                if (appConfig.DbConfig.Curd)
                {
                    fsql.Aop.CurdBefore += (s, e) =>
                    {
                        Console.WriteLine($"{e.Sql}\r\n");
                    };
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabase(DbConfig dbConfig)
        {
            if (!dbConfig.IsCreateDb || dbConfig.DbType == DataType.Sqlite)
            {
                return;
            }

            var db = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.DbType, dbConfig.CreateDbConn)
                    .Build();

            try
            {
                Console.WriteLine("\r\ncreate database started");
                await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
                Console.WriteLine("create database succeed\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"create database failed.\n{e.Message}\r\n");
            }
        }

    }
}
