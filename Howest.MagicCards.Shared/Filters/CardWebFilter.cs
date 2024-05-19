
using Shared.Filters;
using System.ComponentModel.DataAnnotations;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardWebFilter : PaginationFilter
    {
        [StringLength(15, ErrorMessage = "Maximum 15 characters allowed for Name.")]
        public string CardName { get; set; } = string.Empty;
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed for Text.")]
        public string CardText { get; set; } = string.Empty;
        [StringLength(15, ErrorMessage = "Maximum 15 characters allowed for Card type.")]
        public string CardType { get; set; } = string.Empty;
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed for Set Name.")]
        public string Set { get; set; } = string.Empty;
        [StringLength(15, ErrorMessage = "Maximum 15 characters allowed for Artist Name.")]
        public string Artist { get; set; } = string.Empty;
        [StringLength(15, ErrorMessage = "Maximum 15 characters allowed for Rarity.")]
        public string Rarity { get; set; } = string.Empty;
    }
}
