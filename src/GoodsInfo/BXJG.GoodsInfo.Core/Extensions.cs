using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GoodsInfo
{
    public static class Extensions
    {
        /// <summary>
        /// 想物品模块注册具体的物品类型
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GoodsInfoConfiguration AddGoodsType(this GoodsInfoConfiguration config, Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine> func)
        {
            config.AddGoodsInfoTypes.Add(func);
            return config;
        }

        public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(BXJGGoodsInfoCoreConsts.LocalizationSourceName, val);
        }

        public static ILocalizableString ZLJLI(this string str)
        {
            return new LocalizableString(str, BXJGGoodsInfoCoreConsts.LocalizationSourceName);
        }
    }
}
