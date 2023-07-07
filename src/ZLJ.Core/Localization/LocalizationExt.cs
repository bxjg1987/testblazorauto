using Abp.Localization;
using Abp.Localization.Sources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Localization
{
    public static class LocalizationExt
    {
        public static IDictionary<int, string> EnumToDic(this ILocalizationSource localizationSource, Type enumType)
        {
            var dic = new Dictionary<int, string>();

            //这种方式要反射很多次
            //var ary = Enum.GetValues(enumType);
            //foreach (var item in ary)
            //{
            //    dic.Add((int)item, localizationSource.GetString(item.ToString()));
            //}

            var fields = enumType.GetFields();
            if (fields == null || fields.Length <= 1)
                return null;

            for (int i = 1; i < fields.Length; i++)
            {
                var field = fields[i];
                var fieldName = field.Name;
                var localizationKey = fieldName;
                var attr = field.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (attr != null)
                    localizationKey = attr.Description;
                var val = Enum.Parse(enumType, fieldName);
                dic.Add((int)val, localizationSource.GetString(localizationKey));
            }
            return dic;
        }
        public static IDictionary<int, string> EnumToDic(this ILocalizationManager localizationManager, string sourceName, Type enumType)
        {
            return localizationManager.GetSource(sourceName).EnumToDic(enumType);
        }
        public static string GetEnum(this ILocalizationSource localizationSource, Enum obj)
        {
            if (obj == null)
                return null;

            string objName = obj.ToString();
            var localizationKey = objName;
            Type t = obj.GetType();
            FieldInfo fi = t.GetField(objName);
            var attr = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
            if (attr != null)
                localizationKey = attr.Description;

            return localizationSource.GetString(localizationKey);
        }
        public static string GetEnum(this ILocalizationManager localizationManager, string sourceName, Enum obj)
        {
            return localizationManager.GetSource(sourceName).GetEnum(obj);
        }


        public static string GetNullableBool(this ILocalizationSource localizationSource, bool? value)
        {
            return localizationSource.GetString(value.ToString());
        }

        public static IDictionary<bool?, string> GetNullableBool(this ILocalizationSource localizationSource)
        {
            var dic = new Dictionary<bool?, string>();
            dic.Add(false, localizationSource.GetNullableBool(false));
            dic.Add(true, localizationSource.GetNullableBool(true));
            dic.Add(default(bool?), localizationSource.GetString("Unknown"));
            return dic;
        }
    }
}
