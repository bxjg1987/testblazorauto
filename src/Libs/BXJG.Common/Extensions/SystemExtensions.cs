////using hyjiacan.py4n;
//#if NET8_0_OR_GREATER
//using Microsoft.AspNetCore.WebUtilities;
//#endif
//using Microsoft.AspNetCore.Http.Extensions;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace System
{
    public static class SystemExtensions
    {
        //        private const int Second = 1;
        //        private const int Minute = 60 * Second;
        //        private const int Hour = 60 * Minute;
        //        private const int Day = 24 * Hour;
        //        private const int Month = 30 * Day;
        //        public static string ToFriendlyDisplay(this DateTime dateTime)
        //        {
        //#if NET8_0_OR_GREATER
        //            var ts = TimeProvider.System.GetLocalNow().Date - dateTime;
        //#else
        //            var ts = DateTime.Now - dateTime;
        //#endif


        //            var delta = ts.TotalSeconds;
        //            if (delta < 0)
        //            {
        //                return "现在";
        //            }
        //            if (delta < 1 * Minute)
        //            {
        //                return ts.Seconds == 1 ? "1秒钟前" : ts.Seconds + "秒之前";
        //            }
        //            if (delta < 2 * Minute)
        //            {
        //                return "1分钟之前";
        //            }
        //            if (delta < 45 * Minute)
        //            {
        //                return ts.Minutes + "分钟";
        //            }
        //            if (delta < 90 * Minute)
        //            {
        //                return "1小时前";
        //            }
        //            if (delta < 24 * Hour)
        //            {
        //                return ts.Hours + "小时前";
        //            }
        //            if (delta < 48 * Hour)
        //            {
        //                return "昨天";
        //            }
        //            if (delta < 30 * Day)
        //            {
        //                return ts.Days + "天前";
        //            }
        //            if (delta < 12 * Month)
        //            {
        //                var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
        //                return months <= 1 ? "个月之前" : months + "个月之前";
        //            }
        //            else
        //            {
        //                var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
        //                return years <= 1 ? "年之前" : years + "年之前";
        //            }
        //        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        public static string GetDescription(this Enum em)
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

        /// <summary>
        /// 获取类型默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object? GetDefaultValue(this Type type)
        {

            if (type.Name.ToLower() == "void")
                return null;

            if (type.IsValueType)
                return RuntimeHelpers.GetUninitializedObject(type);

            return null;
        }
        //ex.GetBasexxx已经实现了
        ///// <summary>
        ///// 获取最里面的异常
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //public static Exception GetInnermost(this Exception ex)
        //{
        //    if (ex.InnerException == null)
        //        return ex;
        //    else
        //        return ex.InnerException.GetInnermost();
        //}
#if NET8_0_OR_GREATER
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
#endif
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
        /// <summary>
        ///  [Display(Name = "待使用")]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            FieldInfo field = enumType.GetField(value.ToString());

            DisplayAttribute attr = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;

            return attr == null ? value.ToString() : attr.Name;
        }


        /// <summary>
        /// 想url追加参数，若重复则保留多个同名参数,若参数为空，则原样返回url，不会报错
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public static string AddQueryString(this string url, object queryParams)
        {
            if (queryParams == null)
                return url;

            var dictionary = queryParams.GetType()
                .GetProperties()
                //.Where(c => {
                //    if (c.GetValue(queryParams) == c.PropertyType.GetDefaultValue())
                //        return false;
                //    return true;
                //})
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(queryParams, null).ToString());
            //任何项目都可能向后端发起http请求，所以在common库中引入Microsfot.AspNetCore包可以接受
            // System.Web.HttpUtility
            //var qb = new QueryBuilder();
            //foreach (var item in dictionary)
            //{
            //    qb.Add(item.Key,item.Value);
            //}
            //return $"{url}{qb.ToQueryString().Value}";


            var uriBuilder = new UriBuilder(url);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query); // 如果不可用，则需要自己解析查询字符串  

            foreach (var kvp in dictionary)
            {
                query[kvp.Key] = kvp.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri.AbsoluteUri;


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

        /// <summary>
        /// 递归获取父节点的code
        /// a.b.c  得到 [a,a.b]
        /// </summary>
        /// <param name="code"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] RecursionGetParentCode(this string code, string separator = @"\.")
        {
            var ct = Regex.Matches(code, separator).Count;
            if (ct == 0)
                return new[] { code };

            separator = separator.Trim('\\');
            var ary = new string[ct];
            while (ct > 0)
            {
                var idx = code.LastIndexOf(separator);
                code = code.Substring(0, idx);
                ary[ct - 1] = code;
                ct--;
            }
            return ary;
        }
        /// <summary>
        /// 判断类型是否可空
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
        /// <summary>
        /// 判断当前类型是否直接或间接实现了某个接口
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsImplementInterface<TInterface>(this Type type)
        {
            var r = typeof(TInterface).IsAssignableFrom(type);
            //var sdfsd = type.GetInterfaces();
            //var r = sdfsd.Any(x => x.Equals(typeof(TInterface)));
            return r;
        }
        /// <summary>
        /// 判断指定类型是否是数值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this Type type)
        {
            if (type == null)
                return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }


        ///// <summary>
        ///// 判断当前类型是否直接或间接实现了某个接口
        ///// </summary>
        ///// <typeparam name="TInterface"></typeparam>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static bool IsImplementInterface<TInterface>(this object obj)
        //{
        //    return type.GetInterfaces().Any(x => x.Equals(typeof(TInterface)));
        //}

        #region aes加解密
        public static byte[] AesEncrypt(this byte[] data, string key, int keyLength = 256, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, byte[] iv = null)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesGetKey(key, keyLength / 8);
                aes.Mode = mode;
                aes.Padding = padding;

                if (aes.Mode != CipherMode.ECB)
                {
                    aes.IV = iv ?? AesGenerateIV(aes.BlockSize / 8);
                }

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedData = AesPerformCryptography(data, encryptor);
                    if (aes.Mode != CipherMode.ECB)
                    {
                        byte[] result = new byte[aes.IV.Length + encryptedData.Length];
                        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                        Buffer.BlockCopy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);
                        return result;
                    }
                    return encryptedData;
                }
            }
        }

        public static byte[] AesDecrypt(this byte[] data, string key, int keyLength = 256, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, byte[] iv = null)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AesGetKey(key, keyLength / 8);
                aes.Mode = mode;
                aes.Padding = padding;

                byte[] actualData = data;

                if (aes.Mode != CipherMode.ECB)
                {
                    if (iv == null)
                    {
                        iv = new byte[aes.BlockSize / 8];
                        Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
                        actualData = new byte[data.Length - iv.Length];
                        Buffer.BlockCopy(data, iv.Length, actualData, 0, actualData.Length);
                    }
                    aes.IV = iv;
                }

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return AesPerformCryptography(actualData, decryptor);
                }
            }
        }

        private static byte[] AesGetKey(string key, int length)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length < length)
            {
                Array.Resize(ref keyBytes, length);
            }
            else if (keyBytes.Length > length)
            {
                Array.Resize(ref keyBytes, length);
            }
            return keyBytes;
        }

        private static byte[] AesGenerateIV(int length)
        {
            byte[] iv = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        private static byte[] AesPerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public static string AesEncrypt(this string plainText, string key, int keyLength = 256, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, byte[] iv = null)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = plainBytes.AesEncrypt(key, keyLength, mode, padding, iv);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string AesDecrypt(this string encryptedText, string key, int keyLength = 256, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, byte[] iv = null)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = encryptedBytes.AesDecrypt(key, keyLength, mode, padding, iv);
            return Encoding.UTF8.GetString(decryptedBytes);
        }


        #endregion
   
    
    }
}