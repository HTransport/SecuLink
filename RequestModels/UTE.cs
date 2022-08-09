using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Models;

namespace SecuLink.RequestModels
{
    public class UTE : User
    {
        public string Token { get; set; }
        public int EId { get; set; }
    }
}
