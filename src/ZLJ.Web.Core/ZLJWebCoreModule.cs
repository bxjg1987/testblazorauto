using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using ZLJ.Authentication.JwtBearer;
using ZLJ.Configuration;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using BXJG.Utils;
using BXJG.Utils.GeneralTree;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using System.ComponentModel;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.Registration;
using Abp.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Medallion.Threading;
using Medallion.Threading.SqlServer;
using Yitter.IdGenerator;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using BXJG.Utils;
using ZLJ.App.Common;
using ZLJ.App.Admin;
using ZLJ.EntityFrameworkCore;
using BXJG.Common;
using Abp.Configuration.Startup;
using BXJG.Utils.Web;

namespace ZLJ
{
    /*
     * 此文件的重要说明
     * 由于目前我们的主web项目承载所有类型的web
     * Web.Core已经失去了原来的作用，目前它只是作为多个app之间共享组件用，而且这些组件是跟当前业务项目相关的，非业务相关组件，且跟abp相关的，封装在BXJG.Utils中
     * 非业务且与abp无关的，封装在BXJG.Common.RCL中
     * 
     * 由于此项目原本的目的是多个Host共享逻辑的，因此有很多跟web相关的公共配置放这个项目的，
     * 由于现在此项目的作用变了，不要再在这个项目做web相关的配置，而应直接在Host项目中做
     * 原本已经存在的保留不动，将来可以来清理
     * 
     */

    [DependsOn(
        //typeof(AbpHangfireAspNetCoreModule),
        typeof(BXJGUtilsWebModule),
        typeof(ZLJApplicationModule),
        typeof(ZLJEntityFrameworkModule),
        typeof(AbpAspNetCoreModule),
        //typeof(AbpAspNetCoreSignalRModule),
        //typeof(CustomerApplicationModule),
        typeof(CommonApplicationModule))]
    public class ZLJWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule;
        //AppOptions appOptions;
        public ZLJWebCoreModule(IWebHostEnvironment env, ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            this.abpProjectNameEntityFrameworkModule = abpProjectNameEntityFrameworkModule;
        }

        public override void PreInitialize()
        {


            //    使用mvc的时间格式化起为动态api处理时间格式
            //Configuration.Modules.AbpAspNetCore().UseMvcDateTimeFormatForAppServices = true;

            abpProjectNameEntityFrameworkModule.SkipDbSeed=true;

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ZLJConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            

            //Configuration.Modules.AbpAspNetCore()
            //    .CreateControllersForAppServices(
            //        typeof(ZLJApplicationModule).GetAssembly()
            //    );

            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(
            //    typeof(BXJGUtilsModule).Assembly /*, moduleName: "utils", useConventionalHttpVerbs: true*/);

            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGUtilsApplicationModule).Assembly, moduleName: "bxjgutils");
            ////Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(GeneralTreeModule).Assembly);


            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJG.WorkOrder.ApplicationModule).Assembly, "bxjgworkorder");
            //if (Configuration.Modules.BXJGWorkOrder().EnableDefaultWorkOrder)
            //    Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGWorkOrderEmployeeApplicationModule).Assembly, "bxjgemployeeworkorder");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJG.WorkOrder.BXJGCommonApplicationModule).Assembly, "bxjgworkorder");

            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(CommonApplicationModule).Assembly, "common");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(CustomerApplicationModule).Assembly, "customer");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(EmployeeApplicationModule).Assembly, "emp");

            //ConfigureTokenAuth();

            //默认每次启动都会尝试数据库迁移，这里禁用它提高系统启动速度
            // abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            //   Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton); //经过测试没什么卵用
        }

        //private void ConfigureTokenAuth()
        //{
        //    IocManager.Register<TokenAuthConfiguration>();
        //    //IocManager.Register<IWeChatMiniProgramLoginHandler, WeChatMiniProgramLoginHandler>(DependencyLifeStyle.Transient);
        //    var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

        //    tokenAuthConfig.SecurityKey =
        //        new SymmetricSecurityKey(
        //            Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
        //    tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
        //    tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
        //    tokenAuthConfig.SigningCredentials =
        //        new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
        //    tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        //}

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJWebCoreModule).GetAssembly());
            //Lazy<TService>注入
            IocManager.IocContainer.Register(
               Castle.MicroKernel.Registration.Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
            );


            //使用sqlserver作为分布式锁https://github.com/madelson/DistributedLock
            ConfigureDistributedLock();
            //全局雪花id生成器
            ConfigureIdGenarator();

            IocManager.Register<TokenAuthConfiguration>();

            ////Lazy<TService>注入
            //IocManager.IocContainer.Register(
            //   Castle.MicroKernel.Registration.Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
            //);
            ////使用sqlserver作为分布式锁https://github.com/madelson/DistributedLock
            //ConfigureDistributedLock();
            ////全局雪花id生成器
            //ConfigureIdGenarator();
            //IocManager.Register<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//utils已经注册了个 这里可以替换
            //Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//经过测试必须在PreInitialize

            //IocManager.RegService(services =>
            //{
            //    services.Configure<AppOptions>( _appConfiguration  .GetSection("app"));


            //    //services.PostConfigure<AppOptions>(opt =>
            //    //{
            //    //    if (opt.sbslzxpdsc == default)
            //    //        opt.sbslzxpdsc = 960000;

            //    //    if (opt.sbslzxrwjgsc == default)
            //    //        opt.sbslzxrwjgsc = 10000;

            //    //    if (opt.cjjlblts == default)
            //    //        opt.cjjlblts = 90;

            //    //    if (opt.idGeneratorWorkerId == default)
            //    //        opt.idGeneratorWorkerId = 1;
            //    //});
            //});
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>().AddApplicationPartsIfNotAddedBefore(Assembly.GetExecutingAssembly());
            // Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//经过测试没什么卵用
            // var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //IocManager.RegService(services => {
            //    //用ef存储asp.net core数据保护密钥
            //    //https://learn.microsoft.com/zh-cn/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-6.0#persistkeystodbcontext
            //    services.AddDataProtection().PersistKeysToDbContext<ZLJDbContext>();
            //});
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials =
                new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }
       
        /// <summary>
        /// 使用原生的雪花id生成器
        /// </summary>
        private void ConfigureIdGenarator()
        {
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions { WorkerId = ushort.Parse(this._appConfiguration["idGeneratorWorkerId"]) });
            //   IocManager.IocContainer.Register(compo)
            IocManager.IocContainer.Register(Castle.MicroKernel.Registration.Component.For<IIdGenerator>().Instance(YitIdHelper.IdGenInstance));
            //IocManager.Register<IIdGenerator>(Abp.Dependency.DependencyLifeStyle.Singleton, )
            //IocManager.Register()
            //IocManager.IocContainer.Kernel.re
            //IocManager.Register()
            //IocManager.RegService(services => {
            //    services.TryAddSingleton(YitIdHelper.IdGenInstance);
            //});
        }
        /// <summary>
        /// 使用原生的分布式锁
        /// </summary>
        private void ConfigureDistributedLock()
        {
            IocManager.IocContainer.Register(Castle.MicroKernel.Registration.Component.For<IDistributedLockProvider>().Instance(new SqlDistributedSynchronizationProvider(Configuration.DefaultNameOrConnectionString)));

            //IocManager.RegService(srevices =>
            //{
            //    srevices.TryAddSingleton<IDistributedLockProvider>(_ => new SqlDistributedSynchronizationProvider(Configuration.DefaultNameOrConnectionString));
            //});
        }
    }
}