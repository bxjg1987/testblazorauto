using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BXJG.PSI.MasterData.Localization
{
    public static class PSIMasterDataLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BXJGPSIMasterDataCoreConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(PSIMasterDataLocalizationConfigurer).GetAssembly(),
                        "BXJG.PSI.MasterData.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}