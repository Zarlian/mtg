using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        public Task<IQueryable<Card>> GetAllCardsAsync();
        public Task<Card> GetCardByIdAsync(int id);
        public Task<IQueryable<Card>> GetCardsByArtistIdAsync(long id);
    }
}
