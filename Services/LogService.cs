using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using SecuLink.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _dbcont;

        public LogService(ApplicationDbContext dbcont)
        {
            _dbcont = dbcont;
        }

        public async Task Create(string SerialNumber, string MAC)
        {
            _dbcont.Logs.Add(new() { SerialNumber = SerialNumber, MAC = MAC, DOC = DateTime.Now});

            await _dbcont.SaveChangesAsync();
        }

        public async Task<List<LogListItem>> GetList()
        {
            List<Log> logList = await _dbcont.Logs.ToListAsync();

            List<LogListItem> list = new();

            foreach (Log l in logList)
                list.Add(new() { SerialNumber = l.SerialNumber, MAC = l.MAC, DOC = l.DOC });

            return list;
        }
    }
}
