using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;
using BXJG.Utils.Share;

namespace BXJG.PSI
{
    public static class PSILocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(BXJGPSICoreConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(PSILocalizationConfigurer).GetAssembly(),
                        "BXJG.PSI.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
