using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BXJG.Common
{
    
    public static class SecurityHelper
    {
        static RandomNumberGenerator rng = RandomNumberGenerator.Create();
        /// <summary>
        /// a-z A-Z 0-9 生成的随机值 性能略高于Random255
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomBase64(int length = 6)
        {
            var bs = new byte[length];
            rng.GetBytes(bs);
            var str = Convert.ToBase64String(bs)
                .Replace("/", "0")
                .Replace("+", "1")
                .Replace("=", "2")
                .Substring(0, length);
            return str;
        }
        /// <summary>
        /// 从指定字符串中生成随机值，做想生成a-z A-Z 0-9的随机值 RandomBase64方法性能略高
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Random255(string str, byte length = 6)
        {
            var bs = new byte[length];
            rng.GetBytes(bs);
            char[] ary = new char[length];
            for (int i = 0; i < length; i++)
            {
                var j = Convert.ToInt32(bs[i]);
                var k = j % str.Length;
                ary[i] = str[k];
            }
            return new string(ary);
        }
        /// <summary>
        /// 随机纯数字的字符串
        /// 比如生成手机验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumber(byte length = 6)
        {
            //也许有更好的实现方式
            return Random255("0123456789", length);
        }



        public static string GetMD5(this Stream stream)
        {
            byte[] retVal = null;

            using (var md5 = MD5.Create())
            {
                retVal = md5.ComputeHash(stream);
            }

            if (retVal == null || retVal.Length == 0)
                throw new Exception();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GetMD5ByFilePath(this string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                return file.GetMD5();
            }
        }
        /// <summary>
        /// 计算指定字符串的32位md5值
        /// 微信小程序支付签名算法使用的此方式，编码为utf8
        /// 已转换为大写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetMD532(this string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var bs = encoding.GetBytes(str);
            byte[] jmh;
            using (var md5 = MD5.Create())
            {
                jmh = md5.ComputeHash(bs);
            }
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < jmh.Length; i++)
            {
                sb1.Append(jmh[i].ToString("X2"));
            }
            return sb1.ToString();
        }
    }
}
