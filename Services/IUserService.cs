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
        Task<NewUser> CreateNew(string Username);
        Task<NewUser> SelectNewUserByUsername(string Username);
        Task<bool> DeleteNew(string Username);
        Task<User> SelectByUsername(string Username);
        Task<User> SelectById(int Id);
    }
}
