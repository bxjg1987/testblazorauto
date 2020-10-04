using Abp.Dependency;
using Abp.Localization;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    public class EnumManagerFactory : ISingletonDependency
    {
        public readonly EnumManager EnumManager;

        public EnumManagerFactory(ILocalizationManager localizationManager)
        {
            EnumManager = new EnumManager(localizationManager, CoreConsts.LocalizationSourceName);
        }
        //public static readonly EnumManager EnumManager = new EnumManager(base.LocalizationSource);
    }
}
