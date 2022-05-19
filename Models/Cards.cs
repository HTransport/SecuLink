
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Models
{
    public class Cards
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int Pin { get; set; }

        public Cards(int Id=0, string SerialNumber= "&&&&&&&&&", int Pin=-1)
        {
            this.Id = Id;
            this.SerialNumber = SerialNumber;
            this.Pin = Pin;
        }
    }
}
