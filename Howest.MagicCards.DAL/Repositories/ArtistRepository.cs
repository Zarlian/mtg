using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MTGContext _db;

        public ArtistRepository(MTGContext db)
        {
            _db = db;
        }
        public async Task<Artist> GetArtistByIdAsync(long id)
        {
            return await _db.Artists
                                    .Include(a => a.Cards)
                                    .Where(a => a.Id == id)
                                    .FirstAsync();
        }

        public async Task<IQueryable<Artist>> GetAllArtistsAsync()
        {
            IQueryable<Artist> allArtists = _db.Artists.Include(a => a.Cards).Select(a => a);

            return await Task.FromResult(allArtists);
        }
    }
}
