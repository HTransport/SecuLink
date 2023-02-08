using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace SecuLink.Tools
{
    /// <summary>
    /// koristi se za hash-anje i enkripciju stringova
    /// </summary>
    public static class Encryptor
    {
        public static string IV { get; set; }

        public static string Encrypt_Aes(string plainText, string Key)
        {
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(Key);
                aesAlg.IV = Encoding.ASCII.GetBytes(IV);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using StreamWriter swEncrypt = new(csEncrypt);
                
                swEncrypt.Write(plainText);
                encrypted = msEncrypt.ToArray();
            }
            return Encoding.ASCII.GetString(encrypted);
        }

        public static string Decrypt_Aes(string cipherText, string Key)
        {
            string plainText = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(Key);
                aesAlg.IV = Encoding.ASCII.GetBytes(IV);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(Encoding.ASCII.GetBytes(cipherText));
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                plainText = srDecrypt.ReadToEnd();
            }

            return plainText;
        }

        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            SHA256Managed hashstring = new();
            byte[] hash = hashstring.ComputeHash(bytes);

            string result = "";

            foreach (byte x in hash)
                result += string.Format("{0:x2}",x);

            return result;
        }

        public static string GetHashSha256(string text, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text + salt);

            SHA256Managed hashstring = new();
            byte[] hash = hashstring.ComputeHash(bytes);

            string result = "";

            foreach (byte x in hash)
                result += string.Format("{0:x2}", x);

            return result;
        }
    }
}
