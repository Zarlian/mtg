using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly MTGContext _db;

        public CardRepository()
        {
            _db = new MTGContext();
        }

        public async Task<IQueryable<Card>> GetAllCardsAsync()
        {
            IQueryable<Card> allCards = _db.Cards.Select(c => c)
                                                 .OrderBy(c => c.Id);

            return await Task.FromResult(allCards);
        }

        public async Task<Card> GetCardByIdAsync(int id)
        {
            return await _db.Cards.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
