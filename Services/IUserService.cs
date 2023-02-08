using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.ResponseModels;

namespace SecuLink.Services
{
    public interface IUserService
    {
        Task Create(string Username, string FirstName, string LastName, bool Role, string Email, string Password_Enc);
        Task<string> CreateNew(string Username, string FirstName, string LastName, bool Role, string Email, string SerialNumber);
        Task Edit(string CurrentUsername, string Username, string FirstName, string LastName, bool Role, string Email);
        Task Delete(string Username);
        Task DeleteNew(string Username);
        Task<User> SelectById(int Id);
        Task<User> SelectByUsername(string Username);
        Task<NewUser> SelectNewUserByUsername(string Username);
        Task<List<UserListItem>> GetList();
        Task<List<NewUser>> GetListNew();
    }
}
