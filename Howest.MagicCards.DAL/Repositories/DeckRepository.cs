using Howest.MagicCards.DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Howest.MagicCards.DAL.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly IMongoCollection<Deck> _deck;

        public DeckRepository()
        {
            MongoClient client = new MongoClient();
            IMongoDatabase database = client.GetDatabase("mtg_card_deck");
            _deck = database.GetCollection<Deck>("deck");
        }

        public List<Deck> GetAllDecks()
        {
            return _deck.Find(new BsonDocument()).ToList();
        }

        public List<CardInDeck> GetCards()
        {
            return null;
            //return _deck.Find(new BsonDocument()).ToList();
        }

        public void AddCardToDeck(CardInDeck cardInDeck, int maxCardsInDeck)
        {
            List<CardInDeck> deckCards = GetCards();
            if (deckCards.Count >= maxCardsInDeck)
            {
                throw new InvalidOperationException("The deck is already full. Cannot add more cards.");
            }

            CardInDeck foundCard = deckCards.FirstOrDefault(x => x.Id == cardInDeck.Id);
            if (foundCard == null)
            {
                //_deck.InsertOne(cardInDeck);
            }
            else
            {
                throw new ArgumentException("This card is already in the deck");
            }
        }

        public void AddCardToDeck(int deckId, CardInDeck cardInDeck)
        {
            Deck deck =  GetDeck(deckId);

        }

        public void AddDeck(Deck deck)
        {
            List<Deck> decks = GetAllDecks();

            Deck foundDeck = decks.FirstOrDefault(x =>x.Id == deck.Id);
            if (foundDeck == null)
            {
                _deck.InsertOne(deck);
            }
            else
            {
                throw new ArgumentException("Deck already exists");
            }
        }


        public Deck GetDeck(int id)
        {
            Deck foundDeck = _deck.Find(x => x.Id == id).FirstOrDefault();

            if(foundDeck == null)
            {
                throw new ArgumentException("Deck does not exist");
            }
            return foundDeck;
        }

        public void RemoveDeck(int deckId)
        {
            Deck deck = GetDeck(deckId);

            if (deck != null)
            {
                _deck.DeleteOne(x => x.Id == deckId);
            }
            else
            {
                throw new ArgumentException("This deck does not exist.");
            }
        }

    }
}
