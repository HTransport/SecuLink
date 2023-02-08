using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.ResponseModels;
using SecuLink.RequestModels;


namespace SecuLink.Services
{
    public interface ILogService
    {
        Task Create(string SerialNumber, string MAC);
        Task<List<LogListItem>> GetList();
    }
}
