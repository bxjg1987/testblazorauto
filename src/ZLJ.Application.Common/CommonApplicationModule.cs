using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Dictionaries;
using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using BXJG.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ZLJ.Core.Authorization;
using ZLJ.Application.Common.Notification;
using System.Collections.Concurrent;
using ZLJ.Core.Authorization.Users;
using ZLJ.Application.Common.Authorization.Permissions;
using BXJG.Utils.Application;
using ZLJ.Core;
using Abp.Reflection.Extensions;

namespace ZLJ.Application.Common
{


    [DependsOn(
        typeof(ZLJCoreModule),
        typeof(BXJGUtilsApplicationModule),
        typeof(AbpAutoMapperModule))]
    public class CommonApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {

            Configuration.Notifications.Providers.Add<CommonNotifyDefineProvider>();

            Configuration.Authorization.Providers.Add<CommonAppAuthorizationProvider>();
            //注册automapper映射
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            Configuration.Localization.Sources.Add(
              new DictionaryBasedLocalizationSource(ZLJ.Application.Common.Consts.Common,
                  new XmlEmbeddedFileLocalizationDictionaryProvider(
                     Assembly.GetExecutingAssembly(),
                      "ZLJ.Application.Common.Localization.SourceFiles"
                  )
              )
          );
        }

        public override void Initialize()
        {
            //经过测试，这样abp还是无法生成动态webapi，手动提供实现类吧
            //IocManager.Register(typeof(IBXJGShopItemAppService), typeof(BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager, GeneralTreeEntity>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(IBXJGShopFrontItemAppService), typeof(BXJGShopFrontItemAppService<GeneralTreeEntity>), DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(typeof(CommonApplicationModule).GetAssembly());
            //IocManager.Register<YCSDK.Sdf>(DependencyLifeStyle.Transient);
            //IocManager.RegService(service => 
            //{
                //service.AddYCSDK(IocManager.Resolve<IConfiguration>().GetSection("YCSDK"));
            //});
           
            //Configuration.ReplaceService<BXJG.Utils.Notification.PersonNotificationAppService<User>, NotifyAppServie>(DependencyLifeStyle.Transient);

            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile<BXJGShopMapProfile<User>>());

            //IocManager.RegisterBXJGWorkOrderDefaultAdapter<User>();//User为你项目的用户类型

            //集成工单模块需要的员工信息提供器
            //Configuration.ReplaceService<BXJG.WorkOrder.Employee.IEmployeeAppService, WorkOrderEmployeeService<User>>(DependencyLifeStyle.Transient);

            //IocManager.Register<BXJG.WorkOrder.Employee.IEmployeeAppService, WorkOrderEmployeeService<User>>(
            //    DependencyLifeStyle.Transient);


        }

        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<RemoveOldNoticesBackgroundWorker>());
        }
    }
}