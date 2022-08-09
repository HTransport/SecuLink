using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbcont;

        public AuthService(ApplicationDbContext dbcont)
        {
            _dbcont = dbcont;
        }

        public async Task<bool> Authorize(string Username)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(b => b.Username == Username);
            if (u is null)
                return false;
            var a = await _dbcont.Admins.FirstOrDefaultAsync(c => c.UserId == u.Id);
            if (a is not null)
                return false;
            a = new Admin() { UserId = u.Id };
            
            _dbcont.Admins.Add(a);
            await _dbcont.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckIfAuthorized(string Username)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(b => b.Username == Username);
            if (u is null)
                return false;
            var a = await _dbcont.Admins.FirstOrDefaultAsync(c => c.UserId == u.Id);
            if (a is null)
                return false;
            return true;
        }

        public async Task<bool> Unauthorize(string Username)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(b => b.Username == Username);
            if (u is null)
                return false;
            var a = await _dbcont.Admins.FirstOrDefaultAsync(c => c.UserId == u.Id);
            if (a is null)
                return false;

            _dbcont.Admins.Remove(a);
            await _dbcont.SaveChangesAsync();
            return true;
        }
    }
}
