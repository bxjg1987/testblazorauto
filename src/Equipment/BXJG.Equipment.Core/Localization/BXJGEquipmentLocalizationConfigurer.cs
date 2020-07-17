using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BXJG.Equipment.Localization
{
    public static class BXJGEquipmentLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(BXJGEquipmentConst.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(BXJGEquipmentLocalizationConfigurer).GetAssembly(),
                        "BXJG.Equipment.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
