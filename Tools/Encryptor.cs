using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace SecuLink.Tools
{
    /// <summary>
    /// koristi se za hash-anje stringova i (mozda) za enkripciju
    /// </summary>
    public static class Encryptor
    {
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
