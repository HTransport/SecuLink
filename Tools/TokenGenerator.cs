using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Tools
{
    /// <summary>
    /// Koristi se za stvaranje sigurnosnih tokena
    /// </summary>
    public static class TokenGenerator
    {
        private static int Seed 
        {
            get
            {
                int y = DateTime.Now.Year;
                int m = DateTime.Now.Month;
                int d = DateTime.Now.Day;
                int s = DateTime.Now.Second;
                int seed = (y * 365 + y / 4) * 24 * 3600 + m * 30 * 24 * 3600 + d * 24 * 3600 + s;

                return seed; 
            }
        }
        public static string KeyOfTheDay { get; set; }

        public static string GenerateBasic(string Username)
        {
            string token = "";
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            Random random = new(Seed);

            for (int i = 0; i < 14; i++)
            {
                int r = random.Next(0, 61);
                token += chars[r];
            }

            token = Encryptor.GetHashSha256(Username + token, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ");
            return token;
        }

        public static string GenerateBasicUnhashed()
        {
            string key = "";
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            Random random = new(Seed);

            for (int i = 0; i < 32; i++)
            {
                int r = random.Next(0, 61);
                key += chars[r];
            }

            return key;
        }

        public static string GeneratePin(int length)
        {
            string pin = "";
            string chars = "1234567890";

            Random random = new(Seed);

            for (int i = 0; i < length; i++)
            {
                int r = random.Next(0, 9);
                pin += chars[r];
            }
            return pin;
        }
    }
}
