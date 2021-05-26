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
        /// ��������
        /// </summary>
        public IHostEnvironment _env;

        private readonly AppConfig _appConfig;

        /// <summary>
        /// ���캯����ע��
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

            #region NLog ��־

            services.AddLogging(m =>
            {
                m.AddNLog();
            });
            #endregion

            #region Jwt�����֤
            services.AddAuthorization(o =>
            {//��Ȩ
                o.AddPolicy("AdminPolicy", policy => policy.RequireRole("AdminRole").Build());
            })
             .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    var keyByteArray = Encoding.ASCII.GetBytes(_appConfig.JwtConfig.SecurityKey);
                    var signingKey = new SymmetricSecurityKey(keyByteArray);
                    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                    // ����token��֤
                    o.TokenValidationParameters = new TokenValidationParameters
                    {//������֤����
                        IssuerSigningKey = signingKey,//ǩ����Կ
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateLifetime = true,// �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                        ValidIssuer = _appConfig.JwtConfig.Issuer,// Issuer���������ǰ��ǩ��jwt������һ��
                        ValidAudience = _appConfig.JwtConfig.Audience,//�䷢��˭
                        ClockSkew = TimeSpan.FromSeconds(300),// ����ķ�����ʱ��ƫ����
                        RequireExpirationTime = true,// �Ƿ�Ҫ��Token��Claims�б������Expires
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {//��֤ʧ��ʱ����
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {// ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            #endregion

            #region Cors ����

            services.AddCors(o =>
            {//CORS Cross-Origin-Resource-Sharing ����:��Դ��Դ����ͬԴ���ԣ� 
                o.AddPolicy("Limit", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            #endregion


            #region ��������վ����CDN��Nginx��ȡ����ʵIP
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// ����������ı�ͷ�е���Ŀ��
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For������������й��ڷ�������Ŀͻ��˺ͺ����������Ϣ��X-Forwarded-Proto��ԭ������ֵ (HTTP/HTTPS)    
                options.KnownNetworks.Clear(); // ���н���ת��ͷ����֪����ĵ�ַ��Χ�� ʹ����������·��ѡ�� (CIDR) ��ʾ���ṩ IP ��Χ��ʹ��CDNʱӦ���
                options.KnownProxies.Clear(); // ���н���ת��ͷ����֪����ĵ�ַ�� ʹ�� KnownProxies ָ����ȷ�� IP ��ַƥ�䡣ʹ��CDNʱӦ���
            });
            #endregion



            #region �������ݿ�
            services.AddFreeSql(_env, _appConfig);
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            });

            #region IP����
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
            #region AutoFac IOC ����
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
                {//ȫ��ʹ��Container����ע���жϵ�
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

            #region �쳣

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            #endregion

            //app.UseCookiePolicy();//Cookie����
            app.UseForwardedHeaders().UseCertificateForwarding(); // ת������ͷ��֤��
            app.UseStaticFiles();//��̬�ļ�
            app.UseRouting();//·��
            app.UseCors("Limit");//����
            app.UseAuthentication();//��֤
            app.UseAuthorization();//��Ȩ
            app.UseRateLimit();// IP���� 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #region ѩ��ID����
          
            new IdHelperBootstrapper()
                //����WorkerId
                .SetWorkderId(1)
                //ʹ��Zookeeper
                //.UseZookeeper("127.0.0.1:2181", 200, GlobalSwitch.ProjectName)
                .Boot();
            #endregion
        }
    }
}
