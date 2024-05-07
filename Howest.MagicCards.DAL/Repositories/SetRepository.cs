using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SetRepository : ISetRepository
    {
        private readonly MTGContext _db;

        public SetRepository()
        {
            _db = new MTGContext();
        }

        public IQueryable<Set> GetAllSets()
        {
            IQueryable<Set> allSets = _db.Sets.Select(s => s);

            return allSets;
        }
    }
}
