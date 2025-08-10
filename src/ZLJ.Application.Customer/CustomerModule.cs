using Abp.AutoMapper;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Dictionaries;
using Abp.Modules;
using System.Reflection;

using ZLJ.Application.Common;
using ZLJ.Application.Customer.Share;

namespace ZLJ.Application.Customer
{
    [DependsOn(typeof(CommonApplicationModule))]
    public class CustomerModule:AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<CustomerNavigationProvider>();
            Configuration.Authorization.Providers.Add<CustomerAuthorizationProvider>();
            //注册automapper映射
            Configuration.Modules.AbpAutoMapper().Configurators
                .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            Configuration.Localization.Sources.Add(
              new DictionaryBasedLocalizationSource(CustomerConsts.Customer,
                  new XmlEmbeddedFileLocalizationDictionaryProvider(
                     Assembly.GetExecutingAssembly(),
                      "ZLJ.Application.Customer.SourceFiles"
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

    }
}
