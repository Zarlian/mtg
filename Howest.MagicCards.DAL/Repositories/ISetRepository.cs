using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ISetRepository
    {
        public Task<IQueryable<Set>> GetAllSetsAsync();
    }
}
