using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ISetRepository
    {
        Task<IQueryable<Set>> GetAllSetsAsync();
    }
}
