using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Tools
{
    //
    // WIP
    //
    public static class CurrentState
    {
        public static int UsersLS { get; set; }
        public static int NewUsersLS { get; set; }
        public static int CardsLS { get; set; }

        public static void SetLS(int setting, int value)
        {
            switch (setting)
            {
                case 0: UsersLS = value;break;
                case 1: NewUsersLS = value;break;
                case 2: CardsLS = value;break;
            }
        }

        public static void ResetLS()
        {
            UsersLS = NewUsersLS = CardsLS = 0;
        }
    }
}
