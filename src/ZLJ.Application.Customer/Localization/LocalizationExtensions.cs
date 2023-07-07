using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using ZLJ.App.Customer;

namespace System
{
   public static class LocalizationExtensions
    {
        //public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        //{
        //    return LocalizationHelper.Manager.GetEnum(ZLJConsts.LocalizationSourceName, val);
        //}

        public static ILocalizableString GetCustLocalizableString(this object key) => new LocalizableString(key.ToString(),CustConsts.Cust);

        public static string LCust(this object key) => LocalizationHelper.GetString(CustConsts.Cust, key.ToString());
    }
}
