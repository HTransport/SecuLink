using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.ResponseModels;

namespace SecuLink.Services
{
    public interface ICardService
    {
        Task Delete(string SerialNumber);
        Task<Card> Create(string SerialNumber, int UserId);
        Task<Card> SelectByUserId(int UserId);
        Task<List<CU>> GetList();
    }
}
