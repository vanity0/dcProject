using Autofac.Extensions.DependencyInjection;
using DC.Api.Extensions;
using DC.Utils.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System.IO;

namespace DC.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                              .UseJexusIntegration()
                              //.UseUrls("http://192.168.1.101:2025")
                              //.UseKestrel(o =>
                              //{
                              //    o.Listen(IPAddress.Loopback, 2025);
                              //    o.UseSystemd();
                              //})
                              .UseStartup<Startup>();
                })
                .UseNLog();
    }
}
