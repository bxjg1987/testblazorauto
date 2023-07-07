using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Dictionaries;
using Abp.Modules;
using System.Reflection;
using ZLJ.App.Common;

namespace ZLJ.App.Customer
{
    [DependsOn(typeof(CommonApplicationModule))]
    public class CustomerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Localization.ConfigureCust();
            Configuration.Modules.CommonApplication().Apps.TryAdd("cust", new AppInfo { Key = "cust", DisplayName = "客服平台", LoginViewName = "custlogin" });

            Configuration.Authorization.Providers.Add<CustPermissionProvider>();
            // Configuration.Authorization.Providers.Add<PermissionProvider>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            Configuration.Localization.Sources.Add(
               new DictionaryBasedLocalizationSource(CustConsts.Cust,
                   new XmlEmbeddedFileLocalizationDictionaryProvider(
                      Assembly.GetExecutingAssembly(),
                       "ZLJ.App.Customer.Localization.SourceFiles"
                   )
               )
           );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}