using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Models
{
    public class Response
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Response(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
