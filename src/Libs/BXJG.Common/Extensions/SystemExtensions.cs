//using hyjiacan.py4n;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System
{
    public static class SystemExtensions
    {
        /// <summary>
        /// 获取最里面的异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetInnermost(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            else
                return ex.InnerException.GetInnermost();
        }

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
        //public static string GetPinYinFirstLetter(this string chinese, bool toUpper = true)
        //{
        //    var str = string.Empty;
        //    var c = chinese.ToCharArray();
        //    for (int i = 0; i < c.Length; i++)
        //    {
        //        var letter = chinese[i];
        //        if (letter >= 0x4e00 && letter <= 0x9fbb)//是汉字
        //            str += Pinyin4Net.GetFirstPinyin(chinese[i])[0];
        //        else
        //            str += letter;
        //    }

        //    if (toUpper)
        //        str = str.ToUpper();

        //    return str;
        //}
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

        /// <summary>
        /// 一个环（可以理解位首尾相连的数组，索引0规定为环的起始位），从里面取任意一段数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerate"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T[] CaptureClipFromLoop<T>(this T[] list, T index, int count, bool right = true)
        {
            var idx = Array.IndexOf(list, index);
            return list.CaptureClipFromLoop(index, count, right);
        }
        /// <summary>
        /// 一个环（可以理解位首尾相连的数组，索引0规定为环的起始位），从里面取任意一段数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerate"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T[] CaptureClipFromLoop<T>(this T[] list, int index, int count, bool right = true)
        {
            //if (!right)
            //   list = list.Reverse().ToArray();

            T[] result = new T[count];

            var span = new Span<T>(list, 0, list.Length);

            if (right)
            {
                int endLength = Math.Min(index + count, list.Length);
                int accrue = 0;

                for (int i = index; i < endLength; i++)
                {
                    result[accrue] = span[i];
                    accrue++;
                }

                var shortfall = count - accrue;
                for (int i = 0; i < shortfall; i++)
                {
                    result[accrue] = span[i];
                    accrue++;
                }
            }
            else
            {
                //new int[] { 1,2,3,4,5,6,7,8,9,10,11,12 }.CaptureClipFromLoop(1, 3,false);
                int endLength = Math.Max(index - count + 1, 0);
                int accrue = 0;

                for (int i = index; i >= endLength; i--)
                {
                    result[accrue] = span[i];
                    accrue++;
                }

                var shortfall = count - accrue;

                for (int i = list.Length - 1; i > list.Length - 1 - shortfall; i--)
                {
                    result[accrue] = span[i];
                    accrue++;
                }

            }
            return result;
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        public static string GetDescriptionString(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }

        public static bool IsNullOrWhiteSpaceBXJG(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotNullOrWhiteSpaceBXJG(this string str)
        {
            return !str.IsNullOrWhiteSpaceBXJG();
        }
        /// <summary>
        /// 保留几个字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="weiba"></param>
        /// <returns></returns>
        public static string Baoliu(this string str, int length, string weiba = "...")
        {
            if (str.IsNullOrWhiteSpaceBXJG())
                return string.Empty;
            if (str.Length <= length)
                return str;
            return str.Substring(0, length) + weiba;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 十六进制字符串转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(this string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }

        //ReadOnlySequence<byte>转十六禁止字符串
        public static string ToHexString(this ReadOnlySequence<byte> bytes)
        {
            string hexString = string.Empty;

            StringBuilder strB = new StringBuilder();
            var rd = new SequenceReader<byte>(bytes);
            while (rd.TryPeek(out var item))
            {
                strB.Append(item.ToString("X2"));
            }
            hexString = strB.ToString();
            return hexString;
        }
    }
}