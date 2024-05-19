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

        public async Task<List<Deck>> GetAllDecksAsync()
        {
            return await _deck.Find(x => true).ToListAsync();
        }

        public async Task<Deck> GetDeckAsync(string id)
        {
            Deck foundDeck = await _deck.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (foundDeck == null)
            {
                throw new ArgumentException("Deck does not exist");
            }
            return foundDeck;
        }


        public async void AddDeck(Deck deck)
        {
            List<Deck> decks = await GetAllDecksAsync();

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

        public async void UpdateDeckAsync(Deck deck)
        {
            Deck foundDeck = await GetDeckAsync(deck.Id);

            await _deck.ReplaceOneAsync(x => x.Id == deck.Id, deck);
  
        }

        public async void UpdateCardInDeckAsync(string deckId, CardInDeck cardInDeck)
        {
            Deck foundDeck = await GetDeckAsync(deckId);

            if (foundDeck != null)
            {

                CardInDeck foundCardInDeck = foundDeck.Cards.FirstOrDefault(x => x.Id == cardInDeck.Id);

                if (foundCardInDeck != null)
                {
                    foundDeck.Cards.Remove(foundCardInDeck);
                    foundDeck.Cards.Add(cardInDeck);
                    await _deck.ReplaceOneAsync(x => x.Id == deckId, foundDeck);
                }
                else
                {
                    throw new ArgumentException("This card does not exist in this deck.");
                }
            }
            else
            {
                throw new ArgumentException("This deck does not exist.");
            }
        }

        public async void RemoveAllDecksAsync()
        {
            await _deck.DeleteManyAsync(x => true);
        }

        public async void RemoveDeckAsync(string deckId)
        {
            Deck deck = await GetDeckAsync(deckId);

            if (deck != null)
            {
                await _deck.DeleteOneAsync(x => x.Id == deckId);
            }
            else
            {
                throw new ArgumentException("This deck does not exist.");
            }
        }
    }
}
