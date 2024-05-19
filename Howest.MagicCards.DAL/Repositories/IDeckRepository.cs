using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        public Task<List<Deck>> GetAllDecksAsync();
        public Task<Deck> GetDeckAsync(string deckId);
        public void AddDeck(Deck deck);
        public void UpdateDeckAsync(Deck deck);
        void UpdateCardInDeckAsync(string deckId, CardInDeck cardInDeck);
        void RemoveAllDecksAsync();
        public void RemoveDeckAsync(string deckId);        
        

    }
}
