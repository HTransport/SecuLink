using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _dbcont;

        public TokenService(ApplicationDbContext dbcont)
        {
            _dbcont = dbcont;
        }
        public async Task<Token> Create(string Content, int TTL, DateTime DOC, int UserId)
        {
            Token t = new() { Content = Content, TTL_seconds = TTL, DOC = DOC, UserId = UserId};
            _dbcont.Tokens.Add(t);
            await _dbcont.SaveChangesAsync();
            return t;
        }

        public async Task<Token> Create(string Content, DateTime DOC, int UserId)
        {
            Token t = new() { Content = Content, TTL_seconds = 3600, DOC = DOC, UserId = UserId };
            _dbcont.Tokens.Add(t);
            await _dbcont.SaveChangesAsync();
            return t;
        }

        public async Task<Token> Create(string Content, int UserId)
        {
            Token t = new() { Content = Content, TTL_seconds = 3600, DOC = DateTime.Now, UserId = UserId };
            _dbcont.Tokens.Add(t);
            await _dbcont.SaveChangesAsync();
            return t;
        }

        public async Task<bool> Delete(int UserId)
        {
            Token t = await _dbcont.Tokens.FirstOrDefaultAsync(a => a.UserId == UserId);
            if (t is null)
                return false;
            _dbcont.Tokens.Remove(t);
            await _dbcont.SaveChangesAsync();
            return true;
        }

        public async Task<Token> SelectByUserId(int UserId)
        {
            Token t = await _dbcont.Tokens.FirstOrDefaultAsync(a => a.UserId == UserId);
            return t;
        }
    }
}
