using System.Security.Cryptography;

namespace ZLJ.Validation
{
    public static class EncryptHelper
    {
        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="toUpper"></param>
        /// <returns></returns>
        public static string GetMD5(string myString, bool toUpper = false)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);//
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                //这个是很常见的错误，你字节转换成字符串的时候要保证是2位宽度啊，某个字节为0转换成字符串的时候必须是00的，否则就会丢失位数啊。不仅是0，1～9也一样。
                //byte2String += targetData[i].ToString("x");//这个会丢失
                byte2String = byte2String + targetData[i].ToString("x2");
            }

            return toUpper ? byte2String.ToUpper() : byte2String;
        }

        /// <summary>
        /// 在许多显示货币值时，可能需要截取掉后面的0，显示小数值或者整型值。
        /// 数据库中保存的是12.80   在显示中要显示成12.8
        /// 数据库中保存的是12.00   在显示中要显示成12
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal FormatDecimalNum(decimal num)
        {
            var numStr = num.ToString();
            string Number = "";

            if ((numStr.Substring(numStr.Length - 1, 1) == "0") || (numStr.Substring(numStr.Length - 1, 1).Equals(".")))
            {
                Number = FormatDecimalNumStr(numStr.Substring(0, numStr.Length - 1));
            }
            else
            {
                Number = numStr;
            }
            decimal.TryParse(Number, out decimal _decimal);
            return _decimal;
        }
        public static string FormatDecimalNumStr(string numStr)
        {
            if (string.IsNullOrWhiteSpace(numStr)) return "";
            string Number = "";
            try
            {

                if ((numStr.Substring(numStr.Length - 1, 1) == "0") || (numStr.Substring(numStr.Length - 1, 1).Equals(".")))
                {
                    Number = numStr;
                    return FormatDecimalNumStr(numStr.Substring(0, numStr.Length - 1));
                }
                else
                {
                    Number = numStr;
                    return Number;
                }
            }
            catch (System.Exception ex)
            {
                var a = numStr;
                throw;
            }
        }
    }
}
