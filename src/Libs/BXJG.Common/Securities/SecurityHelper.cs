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
        [Obsolete("This method is obsolete. Consider using more secure random generation methods if needed.")]
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
        [Obsolete("This method is obsolete. Consider using more secure random generation methods if needed.")]
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
        [Obsolete("This method is obsolete. Consider using more secure random generation methods if needed.")]
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
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
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

        /// <summary>
        /// Computes the SHA256 hash for the given stream.
        /// </summary>
        /// <param name="stream">The stream to compute the hash for.</param>
        /// <returns>The SHA256 hash as a hexadecimal string.</returns>
        public static string GetSHA256(this Stream stream)
        {
            byte[] retVal = null;

            using (var sha256 = SHA256.Create())
            {
                retVal = sha256.ComputeHash(stream);
            }

            if (retVal == null || retVal.Length == 0)
                throw new Exception("Failed to compute SHA256 hash.");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Computes the SHA256 hash for the file at the specified path.
        /// </summary>
        /// <param name="fileName">The path to the file.</param>
        /// <returns>The SHA256 hash as a hexadecimal string.</returns>
        public static string GetSHA256ByFilePath(this string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return file.GetSHA256();
            }
        }

        /// <summary>
        /// Computes the SHA256 hash for the specified string.
        /// The string is encoded using UTF8 by default.
        /// The hash is returned as an uppercase hexadecimal string.
        /// </summary>
        /// <param name="str">The string to hash.</param>
        /// <param name="encoding">The encoding to use. Defaults to UTF8.</param>
        /// <returns>The SHA256 hash as an uppercase hexadecimal string.</returns>
        public static string GetSHA256String(this string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var bs = encoding.GetBytes(str);
            byte[] jmh;
            using (var sha256 = SHA256.Create())
            {
                jmh = sha256.ComputeHash(bs);
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
