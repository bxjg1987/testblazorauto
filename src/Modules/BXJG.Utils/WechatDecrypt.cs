
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BXJG.Utils
{
    public class WechatDecrypt
    {
        public static DecodedPhoneNumber DecodeEncryptedData(string sessionKey, string encryptedData, string iv)
        {
            var aesCipher = Convert.FromBase64String(encryptedData);
            var aesKey = Convert.FromBase64String(sessionKey);
            var aesIV = Convert.FromBase64String(iv);

            var result = AES_Decrypt(encryptedData, aesIV, aesKey);
            var resultStr = Encoding.UTF8.GetString(result);
            return JsonSerializer.Deserialize<DecodedPhoneNumber>(resultStr);
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

    }

    public class DecodedPhoneNumber
    {

        //
        // 摘要:
        //     用户绑定的手机号（国外手机号会有区号）
        public string phoneNumber { get; set; }
        //
        // 摘要:
        //     没有区号的手机号
        public string purePhoneNumber { get; set; }
        //
        // 摘要:
        //     区号（Senparc注：国别号）
        public string countryCode { get; set; }


        public Watermark watermark { get; set; }

    }

    public class Watermark
    {

        public string appid { get; set; }
        public long timestamp { get; set; }
        public DateTimeOffset DateTimeStamp { get; }
    }

}
//
// 摘要:
//     用户绑定手机号解密类
