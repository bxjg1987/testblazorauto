using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using System.Reflection;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using Abp.Configuration.Startup;
using ZLJ.App.Common;
using Abp.Threading.BackgroundWorkers;


using BXJG.Utils;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Abp.Collections.Extensions;
using Abp.Runtime.Session;
using ZLJ.App.Admin.Sessions;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Dictionaries;
using ZLJ.App.Admin.Authorization.Permissions;

namespace ZLJ.App.Admin
{
    [DependsOn(typeof(CommonApplicationModule))]
    public class ZLJApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<ZLJNavigationProvider>();
            Configuration.Modules.CommonApplication().Apps.TryAdd("admin", new AppInfo { Key = "admin", DisplayName = "后台管理", LoginViewName = "adminlogin" });
            Configuration.Authorization.Providers.Add<ZLJAuthorizationProvider>();
            //注册automapper映射
            Configuration.Modules.AbpAutoMapper().Configurators
                .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            Configuration.Localization.Sources.Add(
              new DictionaryBasedLocalizationSource(AdminConsts.Admin,
                  new XmlEmbeddedFileLocalizationDictionaryProvider(
                     Assembly.GetExecutingAssembly(),
                      "ZLJ.App.Admin.Localization.SourceFiles"
                  )
              )
          );
            //不要替换，AdminSession单独注册算了
            //Configuration.ReplaceService<IAbpSession, AdminSession>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            //经过测试，这样abp还是无法生成动态webapi，手动提供实现类吧
            //IocManager.Register(typeof(IBXJGShopItemAppService), typeof(BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager, GeneralTreeEntity>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(IBXJGShopFrontItemAppService), typeof(BXJGShopFrontItemAppService<GeneralTreeEntity>), DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            //IocManager.RegService(s => {
            //    s.AddHostedService<CustomerReportWorkerRegister>();
            //});

            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile<BXJGShopMapProfile<User>>());

            //IocManager.RegisterBXJGWorkOrderDefaultAdapter<User>();//User为你项目的用户类型

            //集成工单模块需要的员工信息提供器
            //Configuration.ReplaceService<BXJG.WorkOrder.Employee.IEmployeeAppService, WorkOrderEmployeeService<User>>(DependencyLifeStyle.Transient);

            //IocManager.Register<BXJG.WorkOrder.Employee.IEmployeeAppService, WorkOrderEmployeeService<User>>(
            //    DependencyLifeStyle.Transient);
        }


        //string CustomerReportWorkerRegisterId = typeof(CustomerReportWorkerRegister).FullName;
        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();

          // var txq = IocManager.Resolve<IClusterClient>().GetGrain<ITongxinqi>("aaa");
          //  txq.ShezhiPeizhi(new Dictionary<string, string> { });
            //workManager.Add(IocManager.Resolve<clzxzt>());
            //workManager.Add(IocManager.Resolve<sccjjl>());
            //workManager.Add(IocManager.Resolve<EventBackgroundWork>());
            //workManager.Add(IocManager.Resolve<EquipmentReportDataConvert>());
            //workManager.Add(IocManager.Resolve<CustomerReportWorker>());

            //workManager.Add(IocManager.Resolve<SyncStaffToEquipmentInstanceWorkder>());
            //BackgroundJob.Schedule<CustomerReportWorkerRegister>( c => c.StartAsync(), TimeSpan.FromMilliseconds(1));
            //CustomerReportWorkerRegisterId = BackgroundJob.Enqueue<CustomerReportWorkerRegister>(c => c.StartAsync());

            //RecurringJob.AddOrUpdate<CustomerReportWorkerRegister>(CustomerReportWorkerRegisterId, c => c.StartAsync(), Cron.Hourly(Random.Shared.Next(1,59) ));//由于系统启动时可能有很多任务注册，避免大家挤一堆，搞个随机数
            //RecurringJob.Trigger(CustomerReportWorkerRegisterId);
        }

        public override void Shutdown()
        {
            //RecurringJob.RemoveIfExists(CustomerReportWorkerRegisterId);
            //BackgroundJob.Delete(CustomerReportWorkerRegisterId);
        }
    }
}