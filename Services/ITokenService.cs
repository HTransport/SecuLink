using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Models;

namespace SecuLink.Services
{
    public interface ITokenService
    {
        Task Delete(int UserId);
        Task<Token> Create(string Content, int TTL, DateTime DOC, int UserId );
        Task<Token> Create(string Content, DateTime DOC, int UserId);
        Task<Token> Create(string Content, int UserId);
        Task<Token> SelectByUserId(int UserId);
        Task<Token> SelectByContent(string Content);
    }
}
