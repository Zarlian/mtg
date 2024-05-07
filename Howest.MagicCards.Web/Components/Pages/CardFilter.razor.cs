using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class CardFilter
    {

        [Parameter]
        public IEnumerable<RarityReadDTO> Rarities { get; set; }

        [Parameter]
        public IEnumerable<SetReadDTO> Sets { get; set; }

        [Parameter]
        public EventCallback ShowCards { get; set; }

        public NavigationManager NavManager { get; set; }

        private readonly JsonSerializerOptions _jsonOptions;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        public CardFilter()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }
    }
}
