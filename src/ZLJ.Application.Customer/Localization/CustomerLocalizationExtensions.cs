using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using ZLJ.Application;
using ZLJ.Application.Customer.Share;


namespace System
{
   public static class CustomerLocalizationExtensions
    {
        //public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        //{
        //    return LocalizationHelper.Manager.GetEnum(ZLJ.Core.Share.ZLJConsts.LocalizationSourceName, val);
        //}

        public static ILocalizableString GetCustomerLocalizableString(this object key) => new LocalizableString(key.ToString(),CustomerConsts.Customer);

        public static string LCustomer(this object key) => LocalizationHelper.GetString(CustomerConsts.Customer, key.ToString());
    }
}
