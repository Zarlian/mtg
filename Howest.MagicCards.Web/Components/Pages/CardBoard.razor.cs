using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class CardBoard
    {
        [Parameter]
        public IEnumerable<CardReadDTO> Cards { get; set; }
    }
}
