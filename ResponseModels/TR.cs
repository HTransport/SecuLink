using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.ResponseModels
{
    public class TR
    {
        public string Token { get; set; }
        public bool Role { get; set; }
        public string Username { get; set; }
        public TR(string t, bool r, string u)
        {
            Token = t;
            Role = r;
            Username = u;
        }
    }
}
