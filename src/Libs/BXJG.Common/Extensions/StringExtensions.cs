using hyjiacan.py4n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Extensions
{
    public static class StringExtensions
    {
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
    }
}
