using Abp.Localization;
using Abp.Localization.Sources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Localization
{
    public static class BXJGUtilsLocalizationExt
    {
        public static IDictionary<int, string> GetEnum(this ILocalizationSource localizationSource, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new InvalidEnumArgumentException();

            var fields = enumType.GetFields();
            if (fields == null || fields.Length <= 1)
                throw new InvalidEnumArgumentException();

            //这种方式要反射很多次
            //var ary = Enum.GetValues(enumType);
            //foreach (var item in ary)
            //{
            //    dic.Add((int)item, localizationSource.GetString(item.ToString()));
            //}

            var dic = new Dictionary<int, string>();
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
        public static IDictionary<int, string> GetEnum<T>(this ILocalizationSource localizationSource) where T : Enum
        {
            return localizationSource.GetEnum(typeof(T));
        }
        public static IDictionary<int?, string> GetNullableEnum(this ILocalizationSource localizationSource, Type enumType)
        {
            return localizationSource.GetUnknown<int>(localizationSource.GetEnum(enumType));
        }
        public static IDictionary<int?, string> GetNullableEnum<T>(this ILocalizationSource localizationSource) where T : Enum
        {
            return localizationSource.GetUnknown<int>(localizationSource.GetEnum<T>());
        }

        public static IDictionary<int, string> GetEnum(this ILocalizationManager localizationManager, string sourceName, Type enumType)
        {
            return localizationManager.GetSource(sourceName).GetEnum(enumType);
        }
        public static IDictionary<int, string> GetEnum<T>(this ILocalizationManager localizationManager, string sourceName) where T : Enum
        {
            return localizationManager.GetSource(sourceName).GetEnum<T>();
        }
        public static IDictionary<int?, string> GetNullableEnum(this ILocalizationManager localizationManager, string sourceName, Type enumType)
        {
            return localizationManager.GetSource(sourceName).GetNullableEnum(enumType);
        }
        public static IDictionary<int?, string> GetNullableEnum<T>(this ILocalizationManager localizationManager, string sourceName) where T : Enum
        {
            return localizationManager.GetSource(sourceName).GetNullableEnum<T>();
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
        public static string GetEnum<T>(this ILocalizationSource localizationSource, T obj) where T : Enum
        {
            return localizationSource.GetEnum(obj as Enum);
        }

        public static string GetEnum(this ILocalizationManager localizationManager, string sourceName, Enum obj)
        {
            return localizationManager.GetSource(sourceName).GetEnum(obj);
        }
        public static string GetEnum<T>(this ILocalizationManager localizationManager, string sourceName, T obj) where T : Enum
        {
            return localizationManager.GetSource(sourceName).GetEnum<T>(obj);
        }



        public static string GetTrue(this ILocalizationSource localizationSource)
        {
            return localizationSource.GetString(true.ToString());
        }
        public static string GetFalse(this ILocalizationSource localizationSource)
        {
            return localizationSource.GetString(false.ToString());
        }
        public static string GetBool(this ILocalizationSource localizationSource, bool value)
        {
            return localizationSource.GetString(value.ToString());
        }
        public static IDictionary<bool, string> GetBool(this ILocalizationSource localizationSource)
        {
            var dic = new Dictionary<bool, string>();
            dic.Add(false, localizationSource.GetBool(false));
            dic.Add(true, localizationSource.GetBool(true));
            return dic;
        }
        public static IDictionary<bool?, string> GetNullableBool(this ILocalizationSource localizationSource)
        {
            return localizationSource.GetUnknown<bool>(localizationSource.GetBool());
        }




        public static string GetUnknown(this ILocalizationSource localizationSource)
        {
            return localizationSource.GetString("Unknown");
        }
        public static KeyValuePair<T?, string> GetUnknown<T>(this ILocalizationSource localizationSource) where T : struct
        {
            return new KeyValuePair<T?, string>(default(T?), localizationSource.GetUnknown());
        }
        public static IDictionary<T?, string> GetUnknown<T>(this ILocalizationSource localizationSource, IDictionary<T, string> dic) where T : struct
        {
            var dic1 = new Dictionary<T?, string>();
            foreach (var item in dic)
            {
                dic1.Add(item.Key, item.Value);
            }
            var unknown = new KeyValuePair<T?, string>(default(T?), localizationSource.GetUnknown());
            dic1.Add(unknown.Key, unknown.Value);
            return dic1;
        }
    }
}
