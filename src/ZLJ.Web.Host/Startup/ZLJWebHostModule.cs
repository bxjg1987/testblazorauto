using Abp.Modules;
using ZLJ.Configuration;
using Abp.Hangfire;
using Abp.Threading.BackgroundWorkers;
//using ZLJ.App.Admin.WorkOrder.Workload;
using BXJG.Utils;
using ZLJ.EntityFrameworkCore;
using Abp.AspNetCore.SignalR;
using Abp.AspNetCore;
using ZLJ.App.Admin;
//using ZLJ.App.Employee;
using Medallion.Threading.SqlServer;
using Medallion.Threading;
using Yitter.IdGenerator;
using Microsoft.IdentityModel.Tokens;
using ZLJ.Authentication.JwtBearer;
using Abp.AspNetCore.Configuration;
//using BXJG.WorkOrder.EmployeeApplication;
//using BXJG.WorkOrder;
using Castle.MicroKernel.Resolvers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Abp.Runtime.Session;
using Abp.Zero.Configuration;

namespace ZLJ.Web.Host.Startup
{
    [DependsOn(//typeof(AbpHangfireAspNetCoreModule),
               typeof(BXJGUtilsWebModule),
               typeof(ZLJApplicationModule),
               typeof(ZLJEntityFrameworkModule),
               typeof(AbpAspNetCoreModule),
               typeof(AbpAspNetCoreSignalRModule),
               typeof(ZLJWebCoreModule), 
               //typeof(EmployeeApplicationModule),
               typeof(BXJGUtilsRCLModule))]
    public class ZLJWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        //从web.core移动过来的
        ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ZLJWebHostModule(IWebHostEnvironment env, ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();

            //从web.core移动过来的
            this.abpProjectNameEntityFrameworkModule = abpProjectNameEntityFrameworkModule;
            this.httpContextAccessor = httpContextAccessor;
        }
        public override void PreInitialize()
        {
            //多租户开关
            Configuration.MultiTenancy.IsEnabled = ZLJConsts.MultiTenancyEnabled;
            //Configuration.Modules.BXJGUtils().InitDbContext<ZLJDbContext>();
            //Configuration.Navigation.Providers.Add<AdminNavigationProvider>();
            //参考docs/后台作业.txt
            //Configuration.BackgroundJobs.UseHangfire();
            // Use database for language management
            //引入cap动态解析连接字符串的方式会提示事务隔离级别报错，
            //参考：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/4538
            //https://www.cnblogs.com/luckstar007/p/10949811.html
            //Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

            //base.Configuration.Modules.TXDLCore().ApiUrl = _appConfiguration["txdl:apiUrl"]?.TrimEnd('/')+'/';



            #region 从web.core移动过来的
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            //使用mvc的时间格式化起为动态api处理时间格式
            Configuration.Modules.AbpAspNetCore().UseMvcDateTimeFormatForAppServices = true;

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ZLJConsts.ConnectionStringName
            );

            #region 动态webapi
            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(
                   typeof(ZLJApplicationModule).GetAssembly()
               );

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(
                typeof(BXJGUtilsModule).Assembly /*, moduleName: "utils", useConventionalHttpVerbs: true*/);

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGUtilsApplicationModule).Assembly, moduleName: "bxjgutils");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(GeneralTreeModule).Assembly);


            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJG.WorkOrder.ApplicationModule).Assembly, "bxjgworkorder");
            //if (Configuration.Modules.BXJGWorkOrder().EnableDefaultWorkOrder)
            //    Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGWorkOrderEmployeeApplicationModule).Assembly, "bxjgemployeeworkorder");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJG.WorkOrder.BXJGCommonApplicationModule).Assembly, "bxjgworkorder");

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(CommonApplicationModule).Assembly, "common");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(EmployeeApplicationModule).Assembly, "emp");

            #endregion

            ConfigureTokenAuth();

            //默认每次启动都会尝试数据库迁移，这里禁用它提高系统启动速度
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;


            //Lazy<TService>注入
            IocManager.IocContainer.Register(
               Castle.MicroKernel.Registration.Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
            );

            //使用sqlserver作为分布式锁https://github.com/madelson/DistributedLock
            ConfigureDistributedLock();
            //全局雪花id生成器
            ConfigureIdGenarator();

            #endregion
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJWebHostModule).GetAssembly());
         
        }
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>().AddApplicationPartsIfNotAddedBefore(Assembly.GetExecutingAssembly());

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();

            //workManager.Add(IocManager.Resolve<CreateWorkloadRecordMonthlyWorker>());
        }

        #region 从web.core移动过来的
        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            //IocManager.Register<IWeChatMiniProgramLoginHandler, WeChatMiniProgramLoginHandler>(DependencyLifeStyle.Transient);
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
        #endregion


    }
}
