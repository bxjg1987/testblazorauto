using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Dictionaries;
using Abp.Modules;
using System.Reflection;
using ZLJ.App.Common;
using ZLJ.App.Common.Authorization;

namespace ZLJ.App.Customer
{
    [DependsOn(typeof(CommonApplicationModule))]
    public class CustomerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Localization.ConfigureCust();

            Configuration.Authorization.Providers.Add<CustAppAuthorizationProvider>();
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