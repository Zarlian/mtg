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

        public IQueryable<Card> GetAllCards()
        {
            IQueryable<Card> allCards = _db.Cards.Take(100);

            return allCards;
        }
    }
}
