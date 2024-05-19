namespace Howest.MagicCards.Shared.DTO
{
    public record CardInDeckDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
