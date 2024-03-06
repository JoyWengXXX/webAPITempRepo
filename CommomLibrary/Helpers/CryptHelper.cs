using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;


namespace CommonLibrary.Helpers
{
    /// <summary>
    /// 加解密/雜湊相關工具函式
    /// </summary>
    public static class CryptHelper
    {
        //使用AES256進行輸入參數的加密
        public static byte[] AESEncrypt(byte[] plainTextBytes, byte[] keyBytes)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        //使用AES256進行輸入參數的解密
        public static byte[] AESDecrypt(byte[] cipherTextBytes, byte[] keyBytes)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        var plainTextBytes = new byte[cipherTextBytes.Length];
                        var decryptedByteCount = csDecrypt.Read(plainTextBytes, 0, plainTextBytes.Length);
                        return plainTextBytes.Take(decryptedByteCount).ToArray();
                    }
                }
            }
        }

        //使用RSA進行輸入參數的加密
        public static byte[] RSAEncrypt(byte[] plainTextBytes, RSAParameters rsaKeyInfo)
        {
            using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);
                }
        }

        //使用RSA進行輸入參數的解密
        public static byte[] RSADecrypt(byte[] cipherTextBytes, RSAParameters rsaKeyInfo)
        {
            using (var rsa = RSA.Create())
            {
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa.Decrypt(cipherTextBytes, RSAEncryptionPadding.Pkcs1);
                }
        }

        //使用SHA256進行輸入參數的雜湊
        public static byte[] SHA256Hash(byte[] plainTextBytes)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(plainTextBytes);
            }
        }

        //使用SHA512進行輸入參數的雜湊
        public static byte[] SHA512Hash(byte[] plainTextBytes)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(plainTextBytes);
            }
        }
    }
}
