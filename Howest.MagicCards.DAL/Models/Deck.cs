using MongoDB.Bson.Serialization.Attributes;

namespace Howest.MagicCards.DAL.Models
{
    public class Deck
    {
        [BsonElement("_id")]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("cards")]
        public List<CardInDeck> Cards { get; set; }
    }
}
