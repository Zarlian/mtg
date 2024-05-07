using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        List<CardInDeck> GetCards();
        public void AddCardToDeck(CardInDeck cardInDeck, int maxCardsInDeck);
        public void AddCardToDeck(int deckId, CardInDeck cardInDeck);
        List<Deck> GetAllDecks();
        public void AddDeck(Deck deck);
        public void RemoveDeck(int deckId);
        public Deck GetDeck(int deckId);
    }
}
