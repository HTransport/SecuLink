using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Models;

namespace SecuLink.RequestModels
{
    public class ConfirmForm
    {
        public string Username { get; set; }
        public string Pin { get; set; }
        public string Password { get; set; }
    }
}
