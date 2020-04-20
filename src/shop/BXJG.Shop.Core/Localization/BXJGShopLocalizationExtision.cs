using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Localization
{
    public static class BXJGShopLocalizationExtision
    {
        public static ILocalizableString BXJGShopL(this string key)
        {
            return new LocalizableString(key, BXJGShopConsts.LocalizationSourceName);
        }
    }
}
