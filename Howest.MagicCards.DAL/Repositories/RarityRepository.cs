using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class RarityRepository : IRarityRepository
    {
        private readonly MTGContext _db;

        public RarityRepository()
        {
            _db = new MTGContext();
        }

        public IQueryable<Rarity> GetAllRarities()
        {
            IQueryable<Rarity> allRarities = _db.Rarities.Select(r => r);

            return allRarities;
        }
    }
}
