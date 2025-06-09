using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Crystal_Clinic_Mgm.Common.Helper
{
    public static class TextEncryption
    {

        static IConfigurationRoot configRoot = new ConfigurationBuilder().AddUserSecrets(Assembly.GetCallingAssembly()).Build();
        public static string Encrypt(this string plainText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(configRoot.GetRequiredSection("EncryptionKey").Value!);// 16, 24, or 32 bytes for AES-128, AES-192, or AES-256
            aesAlg.IV = Encoding.UTF8.GetBytes(configRoot.GetRequiredSection("EncryptionIV").Value!); // 16 bytes for AES

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new(csEncrypt);
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Decrypt(this string cipherText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(configRoot.GetRequiredSection("EncryptionKey").Value!);// 16, 24, or 32 bytes for AES-128, AES-192, or AES-256
            aesAlg.IV = Encoding.UTF8.GetBytes(configRoot.GetRequiredSection("EncryptionIV").Value!); // 16 bytes for AES
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
