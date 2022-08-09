using Microsoft.EntityFrameworkCore;
using SecuLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Services
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _dbcont;

        public CardService(ApplicationDbContext dbcont)
        {
            _dbcont = dbcont;
        }
        public async Task<Card> Create(string SerialNumber, int UserId)
        {
            Card c = new() { SerialNumber = SerialNumber, UserId=UserId};

            _dbcont.Cards.Add(c);
            await _dbcont.SaveChangesAsync();

            return c;
        }

        public async Task<bool> Delete(string SerialNumber)
        {
            var c = await _dbcont.Cards.FirstOrDefaultAsync(card => card.SerialNumber == SerialNumber);

            if (c is null)
                return false;

            _dbcont.Cards.Remove(c);
            await _dbcont.SaveChangesAsync();

            return true;
        }

        public async Task<Card> SelectByUserId(int UserId)
        {
            var a = await _dbcont.Cards.FirstOrDefaultAsync(card => card.UserId == UserId);
            return a;
        }

        public async Task<Card> SelectBySerialNumber(string SerialNumber)
        {
            var a = await _dbcont.Cards.FirstOrDefaultAsync(card => card.SerialNumber == SerialNumber);
            return a;
        }
    }
}
