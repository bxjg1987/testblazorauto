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
using System.Reflection;

namespace BXJG.AbpZero.Cap.SqlServer
{
    [DependsOn(typeof(BXJGAbpCapEFCoreModule))]
    public class BXJGAbpCapSqlServerModule:AbpModule
    {
        public override void PreInitialize()
        {
            //引入cap动态解析连接字符串的方式会提示事务隔离级别报错，
            //参考：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/4538
            //https://www.cnblogs.com/luckstar007/p/10949811.html
            Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }

    public static class sddfddd
    {
        public static CapOptions UseAbpCapConnectionResolver(this CapOptions cap) {
            cap.RegisterExtension(new zhuce());
            return cap;
        }
    }

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
     */
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