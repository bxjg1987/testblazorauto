using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BXJG.Utils
{
    public class WechatDecrypt
    {
        public static string DecodeEncryptedData(string sessionKey, string encryptedData, string iv)
        {
            var aesCipher = Convert.FromBase64String(encryptedData);
            var aesKey = Convert.FromBase64String(sessionKey);
            var aesIV = Convert.FromBase64String(iv);

            var result = AES_Decrypt(encryptedData, aesIV, aesKey);
            var resultStr = Encoding.UTF8.GetString(result);
            return resultStr;
        }

        private static byte[] AES_Decrypt(String Input, byte[] Iv, byte[] Key)
        {
            SymmetricAlgorithm aes = Aes.Create();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(Input);
                        byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                        Array.Copy(xXml, msg, xXml.Length);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    xBuff = decode2(ms.ToArray());
                }
            }
            catch (CryptographicException e)
            {
                throw e;
            }
            return xBuff;
        }

        private static byte[] decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
        //public static string DecryptPhoneNumber(string sessionkey, string encryptedData, string iv)
        //{


        //    try
        //    {
        //        return AESDecrypt(encryptedData, sessionkey, iv);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public static string AESDecrypt(string encryptedDatatxt, string AesKey, string AesIV)
        //{
        //    try
        //    {
        //        byte[] encryptedData = Convert.FromBase64String(encryptedDatatxt);
        //        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        //        rijndaelCipher.Key = Convert.FromBase64String(AesKey);
        //        rijndaelCipher.IV = Convert.FromBase64String(AesIV);
        //        rijndaelCipher.Mode = CipherMode.CBC;
        //        rijndaelCipher.Padding = PaddingMode.PKCS7;
        //        ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
        //        byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        //        string result = Encoding.Default.GetString(plainText);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}
    }
}
