using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        public Task<List<Deck>> GetAllDecksAsync();
        public Task<Deck> GetDeckAsync(int deckId);
        public void AddDeck(Deck deck);
        public void UpdateDeckAsync(Deck deck);
        void UpdateCardInDeckAsync(int deckId, CardInDeck cardInDeck);
        void RemoveAllDecksAsync();
        public void RemoveDeckAsync(int deckId);        
        

    }
}
