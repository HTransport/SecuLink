using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int Pin { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
