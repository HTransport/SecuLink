using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.ResponseModels;

namespace SecuLink.Services
{
    public interface IAuthService
    {
        Task Authorize(string Username);
        Task Unauthorize(string Username);
        Task<bool> CheckIfAuthorized(string Username);
        Task<int> Authenticate(string token, ITokenService tokenService);
        Task AddReader(string MAC, bool Role);
        Task RemoveReader(string MAC);
        Task<bool> CheckReader(string MAC);
        Task<bool> GetReaderRole(string MAC);
        Task<List<ReaderListItem>> GetReaders();
    }
}