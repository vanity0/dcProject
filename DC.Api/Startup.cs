using Autofac;
using DC.Api.Extensions;
using DC.Api.Models;
using DC.Utils;
using DC.Utils.Configuration;
using DC.Utils.IOC;
using FireflySoft.RateLimit.AspNetCore;
using FireflySoft.RateLimit.Core.InProcessAlgorithm;
using FireflySoft.RateLimit.Core.Rule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coldairarrow.Util;
using DC.Api.JwtAuth;
using DC.Api.Filter;

namespace DC.Api
{
    public class Startup
    {
        /// <summary>
        /// 环境变量
        /// </summary>
        public IHostEnvironment _env;

        private readonly AppConfig _appConfig;

        /// <summary>
        /// 构造函数，注入
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostEnvironment"></param>
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            
            Configuration = configuration;
            _env = hostEnvironment;
            _appConfig = AppSetting.Get<AppConfig>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();

            #region NLog 日志

            services.AddLogging(m =>
            {
                m.AddNLog();
            });
            #endregion

            #region Jwt身份认证
            services.AddAuthorization(o =>
            {//授权
                o.AddPolicy("AdminPolicy", policy => policy.RequireRole("AdminRole").Build());
            })
             .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    var keyByteArray = Encoding.ASCII.GetBytes(_appConfig.JwtConfig.SecurityKey);
                    var signingKey = new SymmetricSecurityKey(keyByteArray);
                    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                    // 设置token验证
                    o.TokenValidationParameters = new TokenValidationParameters
                    {//令牌验证参数
                        IssuerSigningKey = signingKey,//签名秘钥
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,// 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidIssuer = _appConfig.JwtConfig.Issuer,// Issuer，这两项和前面签发jwt的设置一致
                        ValidAudience = _appConfig.JwtConfig.Audience,//颁发给谁
                        ClockSkew = TimeSpan.FromSeconds(300),// 允许的服务器时间偏移量
                        RequireExpirationTime = true,// 是否要求Token的Claims中必须包含Expires
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {//认证失败时调用
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {// 如果过期，则把<是否过期>添加到，返回头信息中
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            #endregion

            #region Cors 跨域

            services.AddCors(o =>
            {//CORS Cross-Origin-Resource-Sharing 跨域:跨源资源共享（同源策略） 
                o.AddPolicy("Limit", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            #endregion


            #region 配置让网站挂在CDN或Nginx获取到真实IP
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// 限制所处理的标头中的条目数
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)    
                options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
                options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
            });
            #endregion



            #region 启动数据库
            services.AddFreeSql(_env, _appConfig);
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            });

            #region IP限流
            services.AddRateLimit(new InProcessTokenBucketAlgorithm(
              new[] {
                    new TokenBucketRule(30,10,TimeSpan.FromSeconds(1))
                    {
                        ExtractTarget = context =>
                        {
                            return (context as HttpContext).Request.Path.Value;
                        },
                        CheckRuleMatching = context =>
                        {
                            return true;
                        },
                        Name="default limit rule",
                    }
              })
          );
            #endregion
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC 容器
            try
            {
                builder.RegisterModule(new AutofacModuleRegister(new List<DllFile>()
                {
                    new DllFile(){ Path = AppContext.BaseDirectory, Name="DC.Service.dll"}
                }
                ));

                builder.RegisterType<JwtService>().As<ITokenService>();
                builder.RegisterType<NLogService>().As<INLogService>();
                builder.RegisterBuildCallback(o =>
                {//全局使用Container进行注入判断等
                    AutofacHelper.Container = o.BeginLifetimeScope();
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            #region 异常

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            #endregion

            //app.UseCookiePolicy();//Cookie策略
            app.UseForwardedHeaders().UseCertificateForwarding(); // 转发请求头和证书
            app.UseStaticFiles();//静态文件
            app.UseRouting();//路由
            app.UseCors("Limit");//跨域
            app.UseAuthentication();//认证
            app.UseAuthorization();//授权
            app.UseRateLimit();// IP限流 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #region 雪花ID分配
          
            new IdHelperBootstrapper()
                //设置WorkerId
                .SetWorkderId(1)
                //使用Zookeeper
                //.UseZookeeper("127.0.0.1:2181", 200, GlobalSwitch.ProjectName)
                .Boot();
            #endregion
        }
    }
}
