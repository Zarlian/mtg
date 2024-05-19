using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.DAL.Repositories;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class DeckEndpoints
    {
        public static void MapDeckEndpoints(this WebApplication deckgroup, string urlPrefix, IMapper mapper, IConfiguration configuration)
        {

            deckgroup.MapGet($"{urlPrefix}/decks", (IDeckRepository deckRepo) =>
                GetAllDecks(deckRepo, mapper))
                .WithTags("Deck actions")
                .Produces<List<DeckDTO>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deckgroup.MapGet($"{urlPrefix}/decks/{{deckId}}", (IDeckRepository deckRepo, string deckId) =>
                GetDeck(deckRepo, deckId, mapper))
                .WithTags("Deck actions")
                .Produces<DeckDetailDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            deckgroup.MapPost($"{urlPrefix}/decks", (IDeckRepository deckRepo, DeckDetailDTO deckDetailDto) =>
                AddDeck(deckRepo, deckDetailDto, mapper))
                .WithTags("Deck actions")
                .Accepts<DeckDetailDTO>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deckgroup.MapPut($"{urlPrefix}/decks/{{deckId}}", (IDeckRepository deckRepo, string deckId, DeckDetailDTO deckDetailDto) =>
                UpdateDeck(deckRepo, deckId, deckDetailDto, mapper))
                .WithTags("Deck actions")
                .Produces<Deck>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            deckgroup.MapPut($"{urlPrefix}/decks/{{deckId}}/cards/{{cardId}}", (IDeckRepository deckRepo, string deckId, int cardId, CardInDeckDTO cardInDeckDto) =>
                UpdateCardInDeck(deckRepo, deckId, cardId, cardInDeckDto,  mapper, configuration))
                .WithTags("Deck actions")
                .Accepts<CardInDeckDTO>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            deckgroup.MapDelete($"{urlPrefix}/decks", (IDeckRepository deckRepo) =>
                RemoveAllDecks(deckRepo))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);


            deckgroup.MapDelete($"{urlPrefix}/decks/{{deckId}}", (IDeckRepository deckRepo, string deckId) =>
                RemoveDeck(deckRepo, deckId))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);
        }

        private static async Task<IResult> GetAllDecks(IDeckRepository deckRepo, IMapper mapper)
        {
            try
            {
                IEnumerable<Deck> decks = await deckRepo.GetAllDecksAsync();
                List<DeckDTO> deckDTOs = mapper.Map<IEnumerable<Deck>, List<DeckDTO>>(decks);
                return Results.Ok(deckDTOs);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error getting decks: {ex.Message}");
            }
        }

        private static async Task<IResult> GetDeck(IDeckRepository deckRepo, string deckId, IMapper mapper)
        {
            try
            {
                Deck deck = await deckRepo.GetDeckAsync(deckId);
                DeckDetailDTO deckDetailDto = mapper.Map<Deck, DeckDetailDTO>(deck);
                return Results.Ok(deckDetailDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error getting deck: {ex.Message}");
            }
        }

        private static IResult AddDeck(IDeckRepository deckRepo, DeckDetailDTO deckDetailDto, IMapper mapper)
        {
            try
            {
                Deck deck = mapper.Map<Deck>(deckDetailDto);

                deckRepo.AddDeck(deck);

                return Results.Ok($"Deck with added");


            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error adding deck: {ex.Message}");
            }
        }

        private static async Task<IResult> UpdateDeck(IDeckRepository deckRepo, string deckId, DeckDetailDTO deckDetailDto, IMapper mapper)
        {
            if(!deckId.Equals(deckDetailDto.Id))
            {
                return Results.BadRequest("Deck id does not match" );
            }

            try
            {
                Deck foundDeck = await deckRepo.GetDeckAsync(deckId);

                if (foundDeck == null)
                {
                    return Results.NotFound($"Deck with id {deckId} not found");
                }

                Deck deck = mapper.Map<Deck>(deckDetailDto);

                deckRepo.UpdateDeckAsync(deck);

                return Results.Ok($"Deck with id {deck.Id} updated");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error updating deck: {ex.Message}");
            }
        }

        private static async Task<IResult> UpdateCardInDeck(IDeckRepository deckRepo, string deckId, int cardId, CardInDeckDTO cardInDeckDto, IMapper mapper, IConfiguration configuration)
        {
            if(cardId != cardInDeckDto.Id)
            {
                return Results.BadRequest("Card id does not match" );
            }

            try
            {
                Deck foundDeck = await deckRepo.GetDeckAsync(deckId);

                if (foundDeck == null)
                {
                    return Results.NotFound($"Deck with id {deckId} not found");
                }

                CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

                deckRepo.UpdateCardInDeckAsync(deckId, cardInDeck);

                return Results.Ok($"Card with id {cardInDeck.Id} with count {cardInDeck.Count} updated in deck");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error updating card in deck: {ex.Message}");
            }
        }

        private static IResult RemoveAllDecks(IDeckRepository deckRepo  )
        {
            try
            {
                deckRepo.RemoveAllDecksAsync();
                return Results.Ok("All decks are deleted");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error removing all decks: {ex.Message}");
            }
        }

        private static IResult RemoveDeck(IDeckRepository deckRepo, string deckId)
        {
            try
            {
                deckRepo.RemoveDeckAsync(deckId);
                return Results.Ok($"Deck with id {deckId} is deleted");
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound($"Error removing deck: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error removing deck: {ex.Message}");
            }
        }
    }
}
