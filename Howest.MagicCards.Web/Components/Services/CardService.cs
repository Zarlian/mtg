using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Components;
using Shared.Wrappers;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Services
{
    public class CardService
    {
        public string _message = string.Empty;

        public IEnumerable<CardReadDTO> _cards = null;
        public IEnumerable<RarityReadDTO> _rarities = null;
        public IEnumerable<SetReadDTO> _sets = null;
        public CardDetailDTO _card = null;

        public int _pageNumber = 1;
        public readonly int _pageSize = 150;
        public int _totalPages = 1;

        public string _ordering = "asc";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public CardService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CardsAPI");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task LoadOnStart()
        {
            _rarities = await GetRarities();
            _sets = await GetSets();
            await GetCards();
        }

        public async Task GetCards(CardWebFilter filter = null)
        {

            string uri = $"Cards?PageNumber={_pageNumber}&PageSize={_pageSize}&SortBy={_ordering}";

            if (filter != null)
            {
                List<string> queryParams = GetQueryParams(filter);

                if (queryParams.Any())
                {
                    uri += $"&{string.Join("&", queryParams)}";
                }
            }

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedResponse<IEnumerable<CardReadDTO>> result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardReadDTO>>>(apiResponse, _jsonOptions);
                _cards = result.Data;
                _totalPages = result.TotalPages;
            }
            else
            {
                _cards = new List<CardReadDTO>();
                _message = $"Error: {response.ReasonPhrase}";
            }
        }

        public async Task GetCard(int cardId)
        {
            _card = await GetCardById(cardId);
        }

        public async Task<CardDetailDTO> GetCardById(int cardId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"cards/{cardId}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Response<CardDetailDTO> result = JsonSerializer.Deserialize<Response<CardDetailDTO>>(apiResponse, _jsonOptions);
                return result.Data;
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
                return new CardDetailDTO();                
            }
        }

        private async Task<IEnumerable<T>> GetApiResponseAsync<T>(string endpointPath)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpointPath);

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

        public async Task<IEnumerable<RarityReadDTO>> GetRarities()
        {
            return await GetApiResponseAsync<RarityReadDTO>("rarity");
        }

        public async Task<IEnumerable<SetReadDTO>> GetSets()
        {
            return await GetApiResponseAsync<SetReadDTO>("set");
        }

        public async Task PreviousPage()
        {
            if (_pageNumber > 1)
            {
                _pageNumber--;
                await GetCards();
            }
        }

        public async Task NextPage()
        {
            if (_pageNumber < _totalPages)
            {
                _pageNumber++;
                await GetCards();
            }
        }

        private static List<string> GetQueryParams(CardWebFilter filter   )
        {
            List<string> queryParams = new List<string>();

            if (!string.IsNullOrEmpty(filter.CardName))
            {
                queryParams.Add($"cardName={Uri.EscapeDataString(filter.CardName)}");
            }

            if (!string.IsNullOrEmpty(filter.CardText))
            {
                queryParams.Add($"cardText={Uri.EscapeDataString(filter.CardText)}");
            }

            if (!string.IsNullOrEmpty(filter.CardType))
            {
                queryParams.Add($"cardType={Uri.EscapeDataString(filter.CardType)}");
            }

            if (!string.IsNullOrEmpty(filter.Set))
            {
                queryParams.Add($"set={Uri.EscapeDataString(filter.Set)}");
            }

            if (!string.IsNullOrEmpty(filter.Artist))
            {
                queryParams.Add($"artist={Uri.EscapeDataString(filter.Artist)}");
            }

            if (!string.IsNullOrEmpty(filter.Rarity))
            {
                queryParams.Add($"rarity={Uri.EscapeDataString(filter.Rarity)}");
            }

            return queryParams;
        }
    }
}
