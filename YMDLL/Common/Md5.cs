using System.Text;
using System;
using System.Security.Cryptography;

namespace YMDLL.Common
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class Md5
    {
        /// <summary>
        /// MD5加密字符串
        /// </summary>
        public static string GetMD5String(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            return BitConverter.ToString(md5data).Replace("-", "").ToLower();
        }
        public static string RSAEncrypt(string encryptString, string username)
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = username.ToLower();
            csp.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(csp);
            byte[] encryptBytes = RSAProvider.Encrypt(ASCIIEncoding.ASCII.GetBytes(encryptString.ToLower()), true);
            string str = "";
            foreach (byte b in encryptBytes)
            {
                str = str + string.Format("{0:x2}", b);
            }
            return str;
        }//加密函数

        public static  string  RSADecrypt(string decryptString, string username)
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = username;
            csp.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(csp);
            int length = (decryptString.Length / 2);
            byte[] decryptBytes = new byte[length];
            for (int index = 0; index < length; index++)
            {
                string substring = decryptString.Substring(index * 2, 2);
                decryptBytes[index] = Convert.ToByte(substring, 16);
            }
            decryptBytes = RSAProvider.Decrypt(decryptBytes, true);
            return ASCIIEncoding.ASCII.GetString(decryptBytes);
        } //解密函数
    }
}
