using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    public static class RandomHelper
    {
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="includeLowercase"></param>
        /// <param name="includeUppercase"></param>
        /// <param name="includeNumber"></param>
        /// <param name="includeSpecial"></param>
        /// <returns></returns>
        public static string GetRandomString(int length, bool includeLowercase = true, bool includeUppercase = true, bool includeNumber = true, bool includeSpecial = false)
        {
            var sb = new StringBuilder();
            var random = new Random();
            var lowercase = "abcdefghijklmnopqrstuvwxyz";
            var uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var number = "0123456789";
            var special = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";
            var all = "";
            if (includeLowercase)
                all += lowercase;
            if (includeUppercase)
                all += uppercase;
            if (includeNumber)
                all += number;
            if (includeSpecial)
                all += special;
            for (int i = 0; i < length; i++)
            {
                var index = random.Next(0, all.Length);
                sb.Append(all[index]);
            }
            return sb.ToString();
        }
    }
}
