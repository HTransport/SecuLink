using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TTL_seconds { get; set; }
        public DateTime DOC { get; set; }
        public int UserId { get; set; }

        public User User;
    }
}
