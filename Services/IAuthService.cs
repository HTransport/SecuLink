using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public interface IAuthService
    {
        Task<bool> Authorize(string Username);
        Task<bool> Unauthorize(string Username);
        Task<bool> CheckIfAuthorized(string Username);
    }
}