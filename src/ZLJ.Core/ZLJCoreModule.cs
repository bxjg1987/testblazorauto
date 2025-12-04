

//using BXJG.WorkOrder;
using Microsoft.Extensions.Configuration;
using BXJG.Utils.Enums;

using Abp.AutoMapper;
using System.Reflection;
using Abp.Threading.BackgroundWorkers;
using ZLJ.Core.Localization;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.MultiTenancy;
using ZLJ.Core.Configuration;
using ZLJ.Core.Timing;

using ZLJ.Core.Features;
using ZLJ.Core.Web;
using ZLJ.Core.Share.Enums;
using Abp.Runtime.Security;
using ZLJ.Notification;
using ZLJ.Core.Share;

namespace ZLJ.Core
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(BXJGUtilsModule)
       // typeof(BXJG.WorkOrder.CoreModule)
        )]
    public class ZLJCoreModule : AbpModule
    {
        //private IConfiguration cfg;
        //public ZLJCoreModule(IHostEnvironment env)
        //{
        //    cfg = env.GetAppConfiguration();
        //}

        public override void PreInitialize()
        { 
            ////多租户开关
            //Configuration.MultiTenancy.IsEnabled = ZLJ.Core.Share.ZLJConsts.MultiTenancyEnabled;
            //try
            //{
            //    cfg = IocManager.Resolve<IConfiguration>();//注意 迁移时为空，迁移时不会依赖webcoreModule，所以那里没问题
            //}
            //catch { cfg = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder()); }
            Configuration.Features.Providers.Add<ZLJFeatureProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators
             .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //IocManager.Register<EquipmentControlCenterConfig>();
            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);
       
           // Configuration.Modules.BXJGWorkOrder().EnableDefaultWorkOrder = false;
            ZLJLocalizationConfigurer.Configure(Configuration.Localization);

            ////多租户开关
            //Configuration.MultiTenancy.IsEnabled = ZLJ.Core.Share.ZLJConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flags gb", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-Hans", "简体中文", "famfamfam-flags cn",true));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = ZLJConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = ZLJConsts.DefaultPassPhrase;

            //注册通知定义提供器
            Configuration.Notifications.Providers.Add<MyAppNotificationProvider>();

            ////替换服务必需是PreInitialize
            // Configuration.ReplaceService<IEmployeeSession, StaffClaimSession>(Abp.Dependency.DependencyLifeStyle.Singleton);

            #region 注册枚举
            //缺个自动注册机制
            Configuration.Modules.BXJGUtils().EnumLocalizationProviders.Add(() => new EnumLocalizationDefine[] {
                new EnumLocalizationDefine(typeof(AdministrativeLevel),"administrativeLevel", locationSourceName: ZLJ.Core.Share.ZLJConsts.LocalizationSourceName),
            });
            #endregion

            //if (cfg != default)//迁移时为空
            //{
            //    Configuration.Modules.BXJGWorkOrder().NoWorkerId = ushort.Parse(cfg["idGeneratorWorkerId"]);
            //}
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJCoreModule).GetAssembly());


            //IocManager.IocContainer.Register(Component.For<IStaffSession>()

            //    .UsingFactoryMethod<StaffClaimSession>((k,c)=> k.Resolve<StaffClaimSession>())
            //                                          //.ImplementedBy<StaffClaimSession>()
            //                                          .LifestyleCustom<MsScopedLifestyleManager>()
            //                                          .Named(new Guid().ToString("N")));//StaffClaimSession的父类已单例注册了，需要重命名下

            //IocManager.IocContainer.Register(Component.For<IStaffSession>()
            //                                          .UsingFactoryMethod((k, c) => k.Resolve<StaffClaimSession>())
            //                                          .LifestyleCustom<MsScopedLifestyleManager>());
            //Configuration.ReplaceService<IEmployeeSession, StaffClaimSession>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Configuration.ReplaceService(typeof(IEmployeeSession), () =>
            //{
            //    IocManager.IocContainer.Register(Component.For<IEmployeeSession>()
            //                                          .UsingFactoryMethod((k, c) => k.Resolve<StaffClaimSession>())
            //                                          .LifestyleCustom<MsScopedLifestyleManager>());


            //});
            //Configuration.ReplaceService<IEmployeeSession, StaffClaimSession>(Abp.Dependency.DependencyLifeStyle.Singleton);

            //IocManager.Register<IStaffLoginManager, StaffLoginManager>(Abp.Dependency.DependencyLifeStyle.Transient);

            ////注册设备中心控制器代理需要的httpClient配置
            //IocManager.RegService(services =>
            //{
            //    services.AddHttpClient(TXDLCoreConsts.HttpClientName,
            //        client =>
            //        {
            //            client.BaseAddress = new Uri(IocManager.Resolve<EquipmentControlCenterConfig>().ApiUrl);
            //        });
            //});

        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}