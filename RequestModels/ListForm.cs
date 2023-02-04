using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.RequestModels
{
    public class ListForm
    {
        public int NumOfElements { get; set; }
        public bool IsNew { get; set; }
        public string Token { get; set; }
    }
}
