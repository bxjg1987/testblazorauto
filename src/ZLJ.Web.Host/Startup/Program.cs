using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using log4net.Config;
using Abp.AspNetCore.Dependency;

namespace ZLJ.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        internal static IHostBuilder CreateHostBuilder(string[] args) =>
          Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  //webBuilder.UseStaticWebAssets();
                  webBuilder.UseStartup<Startup>();
              })
              .UseCastleWindsor(IocManager.Instance.IocContainer);
      

        //public static void Main(string[] args)
        //{
        //    //var sd = Directory.GetCurrentDirectory();
        //    //var sss = AppContext.BaseDirectory;
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    //var cfgHosting = new ConfigurationBuilder()
        //    //     .SetBasePath(Directory.GetCurrentDirectory())
        //    //     //.AddJsonFile("hosting.json", true)
        //    //     .Build();

        //    return WebHost.CreateDefaultBuilder(args)
        //        //不设置的话，默认是Directory.GetCurrentDirectory()，调试时它指向ZLJ.Web.Host，
        //        //但RCL中的静态资源编译后只会复制到debug下，就会出现访问静态资源404
        //        //.UseContentRoot(AppContext.BaseDirectory) 
        //        //参考，说了一堆，反正下面这样配置就正常了
        //        //https://learn.microsoft.com/zh-cn/aspnet/core/razor-pages/ui-class?view=aspnetcore-7.0&tabs=visual-studio#consume-content-from-a-referenced-rcl
        //        //.UseWebRoot("wwwroot")
        //        .UseStaticWebAssets()

        //        //.ConfigureLogging(b =>
        //        //{
        //        //    //b.log()
        //        //    //b.AddBlazorServerLogger();
        //        //})
        //        .UseStartup<Startup>()
        //        //.UseConfiguration(cfgHosting)
        //        .Build();
        //}
    }
}
