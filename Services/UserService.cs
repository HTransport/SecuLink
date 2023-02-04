using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Tools;
using SecuLink.ResponseModels;

namespace SecuLink.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbcont;

        public UserService(ApplicationDbContext dbcont)
        {
            _dbcont = dbcont;
        }
        public async Task Create(string Username, string FirstName, string LastName, bool Role, string Email, string Password_Enc)
        {
            User u = new() { Username = Username, FirstName = FirstName, LastName = LastName, Role = Role, Email = Email,  Password_Enc = Password_Enc};

            _dbcont.Users.Add(u);
            await _dbcont.SaveChangesAsync();
        }

        public async Task Delete(string Username)
        {
            var a = await _dbcont.Users.FirstOrDefaultAsync(x => x.Username == Username);

            _dbcont.Users.Remove(a);
            await _dbcont.SaveChangesAsync();
        }

        public async Task<User> SelectByUsername(string Username)
        {
            var a = await _dbcont.Users.FirstOrDefaultAsync(a => a.Username == Username);
            return a;
        }

        public async Task<string> CreateNew(string Username, string FirstName, string LastName, bool Role, string Email, string SerialNumber)
        {
            NewUser u = new() { Username = Username, FirstName = FirstName, LastName = LastName, Role = Role, Email = Email, SerialNumber = SerialNumber, Pin = TokenGenerator.GeneratePin(8)};

            _dbcont.NewUsers.Add(u);
            await _dbcont.SaveChangesAsync();

            return u.Pin;
        }

        public async Task<NewUser> SelectNewUserByUsername(string Username)
        {
            var a = await _dbcont.NewUsers.FirstOrDefaultAsync(a => a.Username == Username);
            return a;
        }

        public async Task DeleteNew(string Username)
        {
            var a = await _dbcont.NewUsers.FirstOrDefaultAsync(a => a.Username == Username);

            _dbcont.NewUsers.Remove(a);
            await _dbcont.SaveChangesAsync();
        }

        public async Task<List<UserListItem>> GetList()
        {
            var userList = await _dbcont.Users.ToListAsync();

            List<UserListItem> list = new();
            foreach(User u in userList)
            {
                list.Add(new() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Role = u.Role, Email = u.Email});
            }

            return list;
        }

        public async Task<List<NewUser>> GetListNew()
        {
            var list = await _dbcont.NewUsers.ToListAsync();
            return list;
        }

        public async Task Edit(string CurrentUsername, string Username, string FirstName, string LastName, bool Role, string Email)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(a => a.Username == CurrentUsername);
            if (Username is not null && Username != "")
                u.Username = Username;
            if (FirstName is not null && FirstName != "")
                u.FirstName = FirstName;
            if (LastName is not null && LastName != "")
                u.LastName = LastName;
            u.Role = Role;
            if (Email is not null && Email != "")
                u.Email = Email;
            await _dbcont.SaveChangesAsync();
        }
    }
}
