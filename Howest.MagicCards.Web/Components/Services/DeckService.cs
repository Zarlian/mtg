using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Services
{
    public class DeckService
    {

        public event Action OnChange;


        public string _message = string.Empty;

        public IEnumerable<DeckDTO> _decks;
        public DeckDetailDTO _selectedDeck = null;
        public string selectedDeckId = null;
        public string deckName = string.Empty;
        public bool creatingNewDeck = false;

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public DeckService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("DeckAPI");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task GetDecks()
        {

            HttpResponseMessage response = await _httpClient.GetAsync("decks");

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

            NotifyStateChanged();
        }

        public async Task GetDeckById()
        {

            if (selectedDeckId.IsNullOrEmpty())
            {
                _message = "Invalid Deck ID";
                return;
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"decks/{selectedDeckId}");


            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                _selectedDeck = JsonSerializer.Deserialize<DeckDetailDTO>(apiResponse, _jsonOptions);

            }
            else
            {
                _selectedDeck = null;
                _message = $"Error: {response.ReasonPhrase}";
            }

            NotifyStateChanged();

        }

        public async Task DecreaseCardCount(CardInDeckDTO card)
        {
            CardInDeckDTO updatedCard = new CardInDeckDTO
            {
                Id = card.Id,
                Name = card.Name,
                Count = card.Count - 1
            };

            if (updatedCard.Count == 0)
            {
                await RemoveCardFromDeck(card);
            }
            else
            {
                await UpdateCardCount(updatedCard);
            }

        }

        public async Task IncreaseCardCount(CardInDeckDTO card)
        {
            CardInDeckDTO updatedCard = new CardInDeckDTO
            {
                Id = card.Id,
                Name = card.Name,
                Count = card.Count + 1
            };

            if (updatedCard.Count == 0)
            {
                await RemoveCardFromDeck(card);
            }
            else
            {
                await UpdateCardCount(updatedCard);
            }
        }

        public async Task UpdateCardCount(CardInDeckDTO card)
        {
            if (selectedDeckId.IsNullOrEmpty())
            {
                _message = "Invalid Deck ID";
                return;
            }

            string json = JsonSerializer.Serialize(card);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"decks/{selectedDeckId}/cards/{card.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                _message = "Card count updated";
                await GetDeckById();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }

            NotifyStateChanged();
        }

        public async Task RemoveCardFromDeck(CardInDeckDTO card)
        {
            DeckDetailDTO deck = _selectedDeck;
            deck.Cards.Remove(card);

            string json = JsonSerializer.Serialize(deck);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"decks/{deck.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                _message = "Card removed from deck";
                await GetDeckById();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }

            NotifyStateChanged();
        }

        public async Task AddCardToDeck(string cardId, string cardName)
        {
            if(!int.TryParse(cardId, out int cardIdInt))
            {
                _message = "Invalid Card ID";
                return;
            }

            CardInDeckDTO card = new CardInDeckDTO
            {
                Id = cardIdInt,
                Name = cardName,
                Count = 1
            };

            if(_selectedDeck.Cards.Any(x => x.Id == card.Id))
            {
                _message = "Card already in deck";
                NotifyStateChanged();
                return;
            }

            DeckDetailDTO deck = _selectedDeck;
            
            deck.Cards.Add(card);

            string json = JsonSerializer.Serialize(deck);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"decks/{deck.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                _message = "Card added to deck";
                await GetDeckById();
                NotifyStateChanged();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }
            NotifyStateChanged();
        }   

        public async Task ClearDeck()
        {
            DeckDetailDTO deck = _selectedDeck;
            deck.Cards.Clear();

            string json = JsonSerializer.Serialize(deck);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PutAsync($"decks/{deck.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                _message = "Deck cleared";
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }
            NotifyStateChanged();
        }

        public async Task DeleteAllDecks()
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync("decks");

            if (response.IsSuccessStatusCode)
            {
                _message = "All decks Deleted";
                NotifyStateChanged();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }
            NotifyStateChanged();
        }

        public async Task DeleteDeck()
        {
            if (selectedDeckId.IsNullOrEmpty())
            {
                _message = "Invalid Deck ID";
                return;
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync($"decks/{selectedDeckId}");

            if (response.IsSuccessStatusCode)
            {
                selectedDeckId = null;
                _message = "Deck Deleted";
                await GetDecks();
                NotifyStateChanged();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }
            NotifyStateChanged();
        }

        public void CreateDeck()
        {
            creatingNewDeck = true;
        }

        public async Task AddDeck()
        {
            DeckDetailDTO deck = new DeckDetailDTO
            {
                Name = deckName,
                Cards = new List<CardInDeckDTO>()
            };

            string json = JsonSerializer.Serialize(deck);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("decks", content);
            if (response.IsSuccessStatusCode)
            {
                _message = "Deck added";
                creatingNewDeck = false;
                await GetDecks();
                NotifyStateChanged();
            }
            else
            {
                _message = $"Error: {response.ReasonPhrase}";
            }
            NotifyStateChanged();
        }

        public void CancelDeck()
        {
            creatingNewDeck = false;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
