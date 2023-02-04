using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.ResponseModels
{
    public class UserListItem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Role { get; set; }
        public string Email { get; set; }
    }
}
