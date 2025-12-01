using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BXJG.Inventory.Localization
{
    public static class InventoryLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    BXJGInventoryCoreConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(InventoryLocalizationConfigurer).GetAssembly(),
                        "BXJG.Inventory.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}