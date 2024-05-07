
namespace Howest.MagicCards.Shared.DTO
{
    public record DeckDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CardInDeckDTO> Cards { get; set;}
    }
}
