using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Models
{
    public class Request
    {
        public int RequestNumber { get; set; }
        public Request(int RequestNumber)
        {
            this.RequestNumber = RequestNumber;
        }
        public void RequestHandling()
        {
            // 0 - 
        }
    }
}
