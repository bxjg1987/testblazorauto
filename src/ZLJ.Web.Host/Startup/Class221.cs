using Abp;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Modules;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ZLJ.Web.Host.Startup
{

    //public static class sddfddd
    //{
    //    public static CapOptions UseAbpCapConnectionResolver(this CapOptions cap) {
    //        cap.RegisterExtension(new zhuce());
    //        return cap;
    //    }
    //}

    /// <summary>
    /// cap服务注册器
    /// 就是个辅助服务注册的累
    /// 注意尽量靠后的位置调用，因为内部要做服务替换
    /// </summary>
    public class zhuce : ICapOptionsExtension
    {
        public void AddServices(IServiceCollection services)
        {
            services.Replace(new(typeof(IConfigureOptions<SqlServerOptions>), typeof(ssss), ServiceLifetime.Singleton));
        }
    }

    /*
     * cap单例注入 IConfigureOptions<SqlServerOptions> ,ConfigureSqlServerOptions
     * 提供对连接字符串的获取，然后它是直接从ioc中获取dbContext，进而获取连接字符串
     * 而abp中是结合uow和多租户来实现连接字符串处理的
     * 因此这里自定义实现，结合abp的方式实现
     * 
     * 什么辣鸡，玩意，这里就算用IConnectionStringResolver解析得到字符串 也是死的
     * 后续要用都是从选项中去拿连接字符串，多租户场景不适应
     * 
     * 若IConnectionStringResolver用它，必须在abp执行注册服务后，但abp注册服务是最后的步骤，再次之后无法继续注册
     */
    public class ssss : /*ConfigureSqlServerOptions, */IConfigureOptions<SqlServerOptions>
    {
        //IConnectionStringResolver connResolver;
        //public ssss(IConnectionStringResolver connResolver)
        //{
        //    this.connResolver = connResolver;
        //}

        IConfiguration configuration;

        public ssss(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Configure(SqlServerOptions options)
        {
            //静态解析不会报错，动态解析会
            //options.ConnectionString = configuration.GetConnectionString("default");

            //动态解析会报错 参考：https://www.cnblogs.com/luckstar007/p/10949811.html 底部修改 事务隔离级别
            //在efcore层，abp模块配置中调整隔离级别解决这个问题
            options.ConnectionString = configuration.GetConnectionString(ZLJConsts.ConnectionStringName);// connResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs());
        }
    }
}