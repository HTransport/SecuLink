using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Tools
{
    //
    // List<State> !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! KORISTI DEVICE TABLICU ZA TO
    //
    public static class CurrentState
    {
        public static int UsersLS { get; set; }
        public static int NewUsersLS { get; set; }
        public static int CardsLS { get; set; }
        public static int LogLS { get; set; }
        public static int ReaderLS { get; set; }

        public static void SetLS(int setting, int value)
        {
            switch (setting)
            {
                case 0: UsersLS = value; break;
                case 1: NewUsersLS = value; break;
                case 2: CardsLS = value; break;
                case 3: LogLS = value; break;
                case 4: ReaderLS = value; break;
            }
        }

        public static void ResetLS()
        {
            UsersLS = NewUsersLS = CardsLS = LogLS = ReaderLS = 0;
        }
    }
}
