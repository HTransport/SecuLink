using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Tools
{
    /// <summary>
    /// Koristi se za stvaranje sigurnosnih žetona
    /// </summary>
    public static class TokenGenerator
    {
        /// <summary>
        /// Stvara jednostavni sigurnosni token kriptiran koristeći SHA256 Salted
        /// </summary>
        /// <param name="Username">korisničko ime vlasnika tokena</param>
        /// <returns>sigurnosni token</returns>
        public static string GenerateBasic(string Username)
        {
            string token = "";
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            int d = DateTime.Now.Day;
            int s = DateTime.Now.Second;
            Random random = new((y * 365 + y / 4) * 24 * 3600 + m * 30 * 24 * 3600 + d * 24 + 3600 + s); // sjeme za generator je trenutni broj sekundi od početka mjerenja vremena
            int r;
            for (int i = 0; i < 14; i++)
            {
                r = random.Next(0, 61);
                token += chars[r];
            }
            token = Encryptor.GetHashSha256(Username + token, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ");
            return token;
        }
        /// <summary>
        /// Stvara numerički pin određene duljine
        /// </summary>
        /// <param name="length">length</param>
        /// <returns>Numerički pin</returns>
        public static string GeneratePin(int length)
        {
            string pin = "";
            string chars = "1234567890";
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            int d = DateTime.Now.Day;
            int s = DateTime.Now.Second;
            Random random = new((y * 365 + y / 4) * 24 * 3600 + m * 30 * 24 * 3600 + d * 24 + 3600 + s); // sjeme za generator je trenutni broj sekundi od početka mjerenja vremena
            int r;
            for (int i = 0; i < length; i++)
            {
                r = random.Next(0, 9);
                pin += chars[r];
            }
            return pin;
        }
    }
}
