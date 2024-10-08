﻿using Howest.MagicCards.DAL.Models;
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
            IQueryable<Card> allCards = _db.Cards
                                        .Include(c => c.Artist)
                                        .Select(c => c);

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
    }
}
