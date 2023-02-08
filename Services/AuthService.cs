using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using SecuLink.ResponseModels;
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

        public async Task AddReader(string MAC, bool Role)
        {
            Reader r = new() { MAC = MAC, Role = Role };

            _dbcont.Readers.Add(r);

            await _dbcont.SaveChangesAsync();
        }

        public async Task<bool> CheckReader(string MAC)
        {
            var r = await _dbcont.Readers.FirstOrDefaultAsync(R => R.MAC == MAC);

            if (r is null)
                return false;
            return true;
        }

        public async Task RemoveReader(string MAC)
        {
            var r = await _dbcont.Readers.FirstOrDefaultAsync(R => R.MAC == MAC);

            _dbcont.Readers.Remove(r);

            await _dbcont.SaveChangesAsync();
        }

        public async Task<bool> GetReaderRole(string MAC)
        {
            var r = await _dbcont.Readers.FirstOrDefaultAsync(R => R.MAC == MAC);

            return r.Role;
        }

        public async Task<List<ReaderListItem>> GetReaders()
        {
            var readerList = await _dbcont.Readers.ToListAsync();

            List<ReaderListItem> list = new();
            foreach (Reader r in readerList)
            {
                list.Add(new() { MAC = r.MAC, Role = r.Role });
            }

            return list;
        }
    }
}
