using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Models;

namespace SecuLink.RequestModels
{
    public class NUTE : NewUser
    {
        public string Password_Enc { get; set; }
        public string Token { get; set; }
        public int EId { get; set; }
    }
}
