using MongoDB.Bson.Serialization.Attributes;

namespace Howest.MagicCards.DAL.Models
{
    public class CardInDeck
    {
        [BsonElement("_id")]
        public int Id { get; set; }
        [BsonElement("count")]
        public string Count { get; set; }
    }
}
