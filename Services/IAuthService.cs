using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public interface IAuthService
    {
        Task Authorize(string Username);
        Task Unauthorize(string Username);
        Task<bool> CheckIfAuthorized(string Username);
        Task<int> Authenticate(string token, ITokenService tokenService);
    }
}