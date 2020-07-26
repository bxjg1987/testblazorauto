using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BXJG.BaseInfo.Localization
{
    public static class BXJGBaseInfoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(BXJGBaseInfoConst.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(BXJGBaseInfoLocalizationConfigurer).GetAssembly(),
                        "BXJG.BaseInfo.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
