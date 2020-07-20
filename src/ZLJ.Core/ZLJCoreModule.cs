using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using BXJG.Attachment;
using BXJG.CMS;
using BXJG.Equipment;
using BXJG.File;
using BXJG.GeneralTree;
using BXJG.Utils;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.Configuration;
using ZLJ.Localization;
using ZLJ.MultiTenancy;
using ZLJ.Timing;

namespace ZLJ
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(BXJGUtilsModule),
        typeof(GeneralTreeModule),
        typeof(BXJGFileModule), 
        typeof(BXJGAttachmentModule),
        typeof(BXJEquipmentCoreModule))]
    public class ZLJCoreModule : AbpModule
    {
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
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
