using Abp.Localization;
using BXJG.Utils.Localization;
using System;

namespace System
{
   public static class LocalizationExtensions
    {
        //public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        //{
        //    return LocalizationHelper.Manager.GetEnum(ZLJConsts.LocalizationSourceName, val);
        //}

        public static ILocalizableString LICommon(this object key) => new LocalizableString(key.ToString(),ZLJ.App.Common.Consts.Common);

        public static string LCommon(this object key) => LocalizationHelper.GetString(ZLJ.App.Common.Consts.Common, key.ToString());
    }
}
