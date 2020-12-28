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
                str += Pinyin4Net.GetFirstPinyin(chinese[i])[0];
            }

            if (toUpper)
                str = str.ToUpper();

            return str;
        }
    }
}
