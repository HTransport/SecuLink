using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.RequestModels
{
    public class CreateForm
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Role { get; set; }
        public string Email { get; set; }
        public string SerialNumber { get; set; }
    }
}
