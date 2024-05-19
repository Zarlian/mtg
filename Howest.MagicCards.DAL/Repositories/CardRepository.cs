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

        public CardRepository(MTGContext db)
        {
            _db = db;
        }

        public async Task<IQueryable<Card>> GetAllCardsAsync()
        {
            IQueryable<Card> allCards = _db.Cards.Select(c => c)
                                                 .OrderBy(c => c.Id);

            return await Task.FromResult(allCards);
        }

        public async Task<Card> GetCardByIdAsync(int id)
        {
            return await _db.Cards
                .Include(c => c.Rarity)
                .Include(c => c.Set)
                .Include(c => c.Artist)
                .Include(c => c.CardColors)
                    .ThenInclude(cc => cc.Color)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Card>> GetCardsByArtistIdAsync(long id)
        {
            IQueryable<Card> cards = _db.Cards
                                        .Include(c => c.SetCode)
                                        .Include(c => c.RarityCode)
                                        .Include(c => c.CardColors).ThenInclude(cc => cc.Color)
                                        .Include(c => c.CardTypes).ThenInclude(ct => ct.Type)
                                        .Where(c => c.ArtistId == id);

            return await cards.ToListAsync();

        }
    }
}
