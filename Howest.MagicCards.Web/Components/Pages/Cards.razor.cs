using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Components;
using Shared.Wrappers;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class Cards
    {
        private string _message = string.Empty;

        private IEnumerable<CardReadDTO> _cards = null;
        private IEnumerable<RarityReadDTO> _rarities = null;
        private IEnumerable<SetReadDTO> _sets = null;


        private readonly JsonSerializerOptions _jsonOptions;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Cards()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            _rarities = await GetRarities();
            _sets = await GetSets();

            HttpClient httpClient = HttpClientFactory.CreateClient("CardsAPI");

            HttpResponseMessage response = await httpClient.GetAsync("cards");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedResponse<IEnumerable<CardReadDTO>> result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardReadDTO>>>(apiResponse, _jsonOptions);
                _cards = result.Data;
            }
            else
            {
                _cards = new List<CardReadDTO>();
                _message = $"Error: {response.ReasonPhrase}";
            }
        }

        private async Task ShowCards()
        {
            Console.WriteLine("Search button clicked!");
            throw new NotImplementedException();
            /*_cards = await GetCards();*/
        }


        private async Task<IEnumerable<T>> GetApiResponseAsync<T>(string endpointPath)
        {
            HttpClient httpClient = HttpClientFactory.CreateClient("CardsAPI");

            HttpResponseMessage response = await httpClient.GetAsync(endpointPath);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                IEnumerable<T> result = JsonSerializer.Deserialize<IEnumerable<T>>(apiResponse, _jsonOptions);
                return result;
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
                return Enumerable.Empty<T>();

            }
        }

        private async Task<IEnumerable<RarityReadDTO>> GetRarities()
        {
            return await GetApiResponseAsync<RarityReadDTO>("rarity");
        }

        private async Task<IEnumerable<SetReadDTO>> GetSets()
        {
            return await GetApiResponseAsync<SetReadDTO>("set");
        }

        private async Task<IEnumerable<CardReadDTO>> GetCards()
        {
            return await GetApiResponseAsync<CardReadDTO>("cards");
        }

    }
}
