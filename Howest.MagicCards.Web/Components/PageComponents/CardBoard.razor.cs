using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace Howest.MagicCards.Web.Components.PageComponents
{
    public partial class CardBoard
    {
        [Parameter]
        public IEnumerable<CardReadDTO> Cards { get; set; }


    }


}
