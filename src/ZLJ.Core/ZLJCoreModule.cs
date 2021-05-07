using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using BXJG.Attachment;
using BXJG.BaseInfo;
using BXJG.CMS;
//using BXJG.DynamicAssociateEntity;
using BXJG.Equipment;
using BXJG.File;
using BXJG.GeneralTree;
using BXJG.Shop.Customer;
using BXJG.Utils;
using BXJG.WeChat;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.Configuration;
using ZLJ.Localization;
using ZLJ.MultiTenancy;
using ZLJ.Timing;
using BXJG.WorkOrder;

namespace ZLJ
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(BXJGUtilsModule),
        typeof(GeneralTreeModule),
        typeof(BXJGFileModule), 
        typeof(BXJGAttachmentModule),
        typeof(BXJEquipmentCoreModule),
        typeof(BXJGBaseInfoCoreModule),
        typeof(BXJG.WorkOrder.CoreModule),
        typeof(BXJGWeChatModule)/*, 
        typeof(DynamicAssociateEntityModule)*/)]
    public class ZLJCoreModule : AbpModule
    {
        //可以构造函数注入微信模块BXJGWeChatModule
        //然后在构造函数或PreInitialize中对微信进行配置
        //默认情况下无需硬编码配置，只要依赖BXJGWeChatModule模块，然后在配置文件中配置就可以了

        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            ZLJLocalizationConfigurer.Configure(Configuration.Localization);

            // 多租户开关
            //Configuration.MultiTenancy.IsEnabled = ZLJConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.DynamicEntityProperties.Providers.Add<ZLJDynamicEntityPropertyDefinition>();
            //Configuration.Modules.DynamicAssociateEntity().DynamicAssociateEntityDefineGroupProvider
            //Configuration.Modules.BXJGWorkOrder().EnableDefaultWorkOrder = false;
           // BXJG.WorkOrder.BXJGWorkOrderCoreExtensions
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJCoreModule).GetAssembly());
            IocManager.Register<ICustomerLoginManager<User>, CustomerLoginManager<Tenant, Role, User, UserManager>>(Abp.Dependency.DependencyLifeStyle.Transient);
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
