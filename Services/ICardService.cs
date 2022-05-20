using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public interface ICardService
    {
        Task<bool> Delete(string SerialNumber);
        Task<Card> Create(string SerialNumber, int Pin, int UserId);
        Task<Card> SelectByUserId(int UserId);
    }
}
