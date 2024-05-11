using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Shared.Wrappers;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class Deck
    {
        private string _message = string.Empty;

        private IEnumerable<DeckDTO> _decks;

        private readonly JsonSerializerOptions _jsonOptions;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Deck()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = HttpClientFactory.CreateClient("DeckAPI");

            HttpResponseMessage response = await httpClient.GetAsync("decks");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DeckDTO> result = JsonSerializer.Deserialize<IEnumerable<DeckDTO>>(apiResponse, _jsonOptions);
                _decks = result;
            }
            else
            {
                _decks = new List<DeckDTO>();
                _message = $"Error: {response.ReasonPhrase}";
            }
        }

        private void GetDeck()
        {

        }
    }
}
