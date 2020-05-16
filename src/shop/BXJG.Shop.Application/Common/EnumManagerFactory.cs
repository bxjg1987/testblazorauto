using Abp.Dependency;
using Abp.Localization;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Common
{
    public class EnumManagerFactory : ISingletonDependency
    {
        public readonly EnumManager EnumManager;

        public EnumManagerFactory(ILocalizationManager localizationManager)
        {
            EnumManager = new EnumManager(localizationManager, BXJGShopConsts.LocalizationSourceName);
        }
        //public static readonly EnumManager EnumManager = new EnumManager(base.LocalizationSource);
    }
}
