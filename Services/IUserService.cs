using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public interface IUserService
    {
        Task<bool> Delete(int Id);
        Task<User> Create(string Username, string Password_Enc);
        Task<User> SelectByUsername(string Username);
    }
}
