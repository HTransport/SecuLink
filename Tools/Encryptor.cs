using System;
using System.Text;
using System.Security.Cryptography;
using XSystem.Security.Cryptography;

namespace SecuLink.Tools
{
    public static class Encryptor
    {
        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            System.Security.Cryptography.SHA256Managed hashstring = new();
            byte[] hash = hashstring.ComputeHash(bytes);
            string result = string.Empty;
            foreach (byte x in hash)
                result += string.Format("{0:x2}",x);
            return result;
        }
    }
}
