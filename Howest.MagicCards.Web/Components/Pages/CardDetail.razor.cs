using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class CardDetail
    {
        //private string _message = string.Empty;
        //CardDetailDTO _card = null;

        //[Parameter]
        //public int CardId { get; set; }

        //private readonly JsonSerializerOptions _jsonOptions;

        //[Inject]
        //public IHttpClientFactory HttpClientFactory { get; set; }
        //[Inject]
        //public NavigationManager NavManager { get; set; }

        //public CardDetail()
        //{
        //    _jsonOptions = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true,
        //    };
        //}

        //protected override async Task OnInitializedAsync()
        //{
        //    HttpClient httpClient = HttpClientFactory.CreateClient("CardsAPI");

        //    HttpResponseMessage response = await httpClient.GetAsync($"cards/{CardId}");

        //    string apiResponse = await response.Content.ReadAsStringAsync();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        Response<CardDetailDTO> result = JsonSerializer.Deserialize<Response<CardDetailDTO>>(apiResponse, _jsonOptions);
        //        _card = result.Data;
        //    }
        //    else
        //    {
        //        _card = new CardDetailDTO();
        //        _message = $"Error: {response.ReasonPhrase}";
        //    }
        //}

        //public void NavigateToHomePage()
        //{
        //    navigationManager.NavigateTo($"/");
        //}
    }
}
