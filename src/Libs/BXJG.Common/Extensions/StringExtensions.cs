using hyjiacan.py4n;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// 反序列化包含object类型属性的对象
        /// <see href="https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-6-0#deserialize-inferred-types-to-object-properties "/>
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="json"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TValue? DeserializeObject<TValue>(this string json, JsonSerializerOptions? options = null)
        {
            if (options == default)
                options = new JsonSerializerOptions();

            options.Converters.Add(new ObjectToInferredTypesConverter());

            return JsonSerializer.Deserialize<TValue?>(json, options);
        }
        public static bool NavPropertySplit(this string sortString, out string ss, params string[] temp)
        {

            ss = sortString;
            var temp1 = "";

            foreach (var item in temp)
            {
                if (!string.IsNullOrWhiteSpace(temp1))
                    temp1 += ".";
                temp1 += item;
                if (sortString.StartsWith(temp1, StringComparison.OrdinalIgnoreCase))
                {
                    sortString = sortString.Replace(temp1, temp1 + ".", StringComparison.OrdinalIgnoreCase);
                }
                //else
                //    return false;
            }
            if (ss == sortString)
                return false;

            ss = sortString;
            //return sortString;
            return true;
        }
        public static string GetPinYinFirstLetter(this string chinese, bool toUpper = true)
        {
            var str = string.Empty;
            var c = chinese.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                var letter = chinese[i];
                if (letter >= 0x4e00 && letter <= 0x9fbb)//是汉字
                    str += Pinyin4Net.GetFirstPinyin(chinese[i])[0];
                else
                    str += letter;
            }

            if (toUpper)
                str = str.ToUpper();

            return str;
        }
        /// <summary>
        /// <para>More convenient than using T.TryParse(string, out T). 
        /// Works with primitive types, structs, and enums.
        /// Tries to parse the string to an instance of the type specified.
        /// If the input cannot be parsed, null will be returned.
        /// </para>
        /// <para>
        /// If the value of the caller is null, null will be returned.
        /// So if you have "string s = null;" and then you try "s.ToNullable...",
        /// null will be returned. No null exception will be thrown. 
        /// </para>
        /// <author>Contributed by Taylor Love (Pangamma)</author>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_self"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this string p_self) where T : struct
        {
            if (!string.IsNullOrEmpty(p_self))
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                if (converter.IsValid(p_self)) return (T)converter.ConvertFromString(p_self);
                if (typeof(T).IsEnum) { T t; if (Enum.TryParse<T>(p_self, out t)) return t; }
            }

            return null;
        }
        //public static bool IsChinese(this string text) {
        //    char[] c = text.ToCharArray();

        //    for (int i = 0; i < c.Length; i++)
        //        if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
        //            Console.WriteLine("是汉字");
        //        else
        //            Console.WriteLine("不是汉字");
        //}

        /// <summary>
        /// 将路径中的反斜杠转换为url中的正斜杠，已适配多操作系统
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DirectorySeparatorChar2UrlSeparatorChar(this string str)
        {
            return str.Replace(Path.DirectorySeparatorChar, '/');
        }
        /// <summary>
        /// 将url中的正斜杠转换为路径中的反斜杠，已适配多操作系统
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlSeparatorChar2DirectorySeparatorChar(this string str)
        {
            return str.Replace('/', Path.DirectorySeparatorChar);
        }
        ///// <summary>
        ///// 将json字符串转换为字典
        ///// 默认属性名忽略大小写
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="comparer"></param>
        ///// <returns></returns>
        //public static Dictionary<string, object> JsonStringToDic(this string str, IEqualityComparer<string>? comparer = default)
        //{
        //    comparer = comparer ?? StringComparer.OrdinalIgnoreCase;
         
        //    return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(str).ToDictionary(kvp => kvp.Key, kvp => kvp.Value, comparer);
        //}
    }
}