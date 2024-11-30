using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ZLJ.Core.Localization
{
    public static class ZLJLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(ZLJ.Core.Share.ZLJConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ZLJLocalizationConfigurer).GetAssembly(),
                        "ZLJ.Core.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
