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
