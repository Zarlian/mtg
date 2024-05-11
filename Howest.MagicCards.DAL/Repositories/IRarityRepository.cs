using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IRarityRepository
    {
        public Task<IQueryable<Rarity>> GetAllRaritiesAsync();
    }
}
