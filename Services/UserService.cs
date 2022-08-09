using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Tools;

namespace SecuLink.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbcont;

        public UserService(ApplicationDbContext dbcont)
        {
            _dbcont = dbcont;
        }
        public async Task<User> Create(string Username, string Password_Enc)
        {
            User u = new() { Username = Username, Password_Enc = Password_Enc};
            _dbcont.Users.Add(u);
            await _dbcont.SaveChangesAsync();
            return u;
        }

        public async Task<User> Create(int Id, string Username, string Password_Enc, int a)
        {
            switch (a)
            {
                case 0: User u1 = new() { Id = Id, Username = Username, Password_Enc = Password_Enc };return u1;
                default:break;
            }
            User u = new() { Username = Username, Password_Enc = Password_Enc };
            _dbcont.Users.Add(u);
            await _dbcont.SaveChangesAsync();
            return u;
        }

        public async Task<bool> Delete(int Id)
        {
            var a = await _dbcont.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (a != null)
            {
                _dbcont.Users.Remove(a);
                await _dbcont.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> SelectByUsername(string Username)
        {
            var a = await _dbcont.Users.FirstOrDefaultAsync(a => a.Username == Username);
            return a;
        }

        public async Task<User> SelectById(int Id)
        {
            var a = await _dbcont.Users.FirstOrDefaultAsync(a => a.Id == Id);
            return a;
        }

        public async Task<NewUser> CreateNew(string Username)
        {
            NewUser u = new() { Username = Username, Pin = TokenGenerator.GeneratePin(8)};
            _dbcont.NewUsers.Add(u);
            await _dbcont.SaveChangesAsync();
            return u;
        }

        public async Task<NewUser> SelectNewUserByUsername(string Username)
        {
            var a = await _dbcont.NewUsers.FirstOrDefaultAsync(a => a.Username == Username);
            return a;
        }

        public async Task<bool> DeleteNew(string Username)
        {
            var a = await _dbcont.NewUsers.FirstOrDefaultAsync(a => a.Username == Username);
            if (a != null)
            {
                _dbcont.NewUsers.Remove(a);
                await _dbcont.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
