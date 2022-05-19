
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password_Enc { get; set; }
        public int CardId { get; set; }

        public Users(int Id=0, string Username="&&&&&&&&&", string Password_Enc="%%%%%%%%%%", int CardId=0)
        {
            this.Id = Id;
            this.Username = Username;
            this.Password_Enc = Password_Enc;
            this.CardId = CardId;
        }
    }
}
