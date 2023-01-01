using Abp;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Modules;
using BXJG.AbpZero.Cap.EFCore;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace BXJG.AbpZero.Cap.SqlServer
{
    [DependsOn(typeof(BXJGAbpCapEFCoreModule))]
    public class BXJGAbpCapSqlServerModule:AbpModule
    {
    }

    public static class sddfddd
    {
        public static CapOptions UseAbpConnectionResolver(this CapOptions cap) {
            cap.RegisterExtension(new zhuce());
            return cap;
        }
    }
    public class zhuce : ICapOptionsExtension
    {
        public void AddServices(IServiceCollection services)
        {
            services.Replace(new(typeof(IConfigureOptions<SqlServerOptions>), typeof(ssss), ServiceLifetime.Singleton));
        }
    }
    public class ssss : /*ConfigureSqlServerOptions, */IConfigureOptions<SqlServerOptions>
    {

        IConnectionStringResolver connResolver;
        public ssss(IConnectionStringResolver connResolver)
        {
            this.connResolver = connResolver;
        }

        public void Configure(SqlServerOptions options)
        {
            //静态解析不会报错，动态解析会
            //options.ConnectionString = configuration.GetConnectionString("default");

            //动态解析会报错 参考：https://www.cnblogs.com/luckstar007/p/10949811.html 底部修改 事务隔离级别
            //在efcore层，abp模块配置中调整隔离级别解决这个问题
            options.ConnectionString = connResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs());
        }
    }
}