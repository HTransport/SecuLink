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

        public async Task Authorize(string Username)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(b => b.Username == Username);

            u.Role = true;

            await _dbcont.SaveChangesAsync();
        }

        public async Task<bool> CheckIfAuthorized(string Username)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(b => b.Username == Username);

            return u.Role;
        }

        public async Task Unauthorize(string Username)
        {
            var u = await _dbcont.Users.FirstOrDefaultAsync(b => b.Username == Username);

            u.Role = true;

            await _dbcont.SaveChangesAsync();
        }
        public async Task<int> Authenticate(string token, ITokenService tokenService)
        {
            var t = await tokenService.SelectByContent(token);

            if (t is null)
                return 401;

            t = await tokenService.SelectByUserId(t.UserId);

            if (t is null)
                return 401;

            if (t.Content != token)
                return 401;

            DateTime dt = DateTime.Now;
            if (t.DOC.AddSeconds(Convert.ToDouble(t.TTL_seconds)) < dt)
            {
                await tokenService.Delete(t.UserId);
                return 401;
            }

            return 200;
        }
    }
}
