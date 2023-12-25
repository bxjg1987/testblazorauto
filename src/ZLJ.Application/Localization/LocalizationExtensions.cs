using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using ZLJ.App.Admin;

namespace System
{
   public static class LocalizationExtensions
    {
        //public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        //{
        //    return LocalizationHelper.Manager.GetEnum(ZLJConsts.LocalizationSourceName, val);
        //}

        public static ILocalizableString GetAdminLocalizableString(this object key) => new LocalizableString(key.ToString(),AdminConsts.Admin);

        public static string LAdmin(this object key) => LocalizationHelper.GetString(AdminConsts.Admin, key.ToString());
    }
}
