
namespace Howest.MagicCards.Shared.DTO
{
    public record DeckDetailDTO : DeckDTO
    {
        public List<CardInDeckDTO> Cards { get; set;}
    }
}
