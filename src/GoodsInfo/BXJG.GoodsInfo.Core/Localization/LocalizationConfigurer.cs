using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BXJG.GoodsInfo.Localization
{
    public static class LocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(BXJGGoodsInfoCoreConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(LocalizationConfigurer).GetAssembly(),
                        "BXJG.GoodsInfo.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
