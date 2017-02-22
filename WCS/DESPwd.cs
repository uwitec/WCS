using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace WCS
{
    /// <summary>
    /// DESEncrypt加密解密算法
    /// </summary>
    public sealed class DESPwd
    {
        private DESPwd()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private static string key = "nbytyywcs";
        private static string key2 = "nbingtewms";

        /**//// <summary>
        /// 对称加密解密的密钥
        /// </summary>
        public static string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /**//// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
       public static string DesEncrypt(string encryptString)
       {
           if (encryptString.Length == 0)
           {
               return encryptString;
           }
           try
           {
               byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
               byte[] keyIV = keyBytes;
               byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
               DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
               MemoryStream mStream = new MemoryStream();
               CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
               cStream.Write(inputByteArray, 0, inputByteArray.Length);
               cStream.FlushFinalBlock();
               return Convert.ToBase64String(mStream.ToArray());
           }
           catch (Exception e)
           {
               return "F";
           }
        }

        /**//// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            if (decryptString.Length == 0)
            {
                return decryptString;
            }
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception e)
            {
                return "F";
            }
            
        }
        /**/
        /// <summary>
        /// DES加密2
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt2(string encryptString)
        {
            if (encryptString.Length == 0)
            {
                return encryptString;
            }
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key2.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception e)
            {
                return "F";
            }
        }

        /**/
        /// <summary>
        /// DES解密2
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt2(string decryptString)
        {
            if (decryptString.Length == 0)
            {
                return decryptString;
            }
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key2.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception e)
            {
                return "F";
            }

        }

    }
}
