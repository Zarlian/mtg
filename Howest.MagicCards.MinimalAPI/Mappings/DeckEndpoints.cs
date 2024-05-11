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
                .Produces<List<Deck>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deckgroup.MapPost($"{urlPrefix}/decks", (IDeckRepository deckRepo, DeckDTO deckDto) =>
                AddDeck(deckRepo, deckDto, mapper))
                .WithTags("Deck actions")
                .Accepts<DeckDTO>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deckgroup.MapGet($"{urlPrefix}/decks{{id}}", (IDeckRepository deckRepo, int deckId) => 
                GetDeck(deckRepo, deckId, mapper))
                .WithTags("Deck actions")
                .Produces<Deck>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deckgroup.MapPost($"{urlPrefix}/decks{{id}}", (IDeckRepository deckRepo, int deckId, CardInDeckDTO cardInDeckDto) =>
                AddCardToDeck(deckRepo, cardInDeckDto, deckId,  mapper, configuration))
                .WithTags("Card actions")
                .Accepts<CardInDeckDTO>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            deckgroup.MapDelete($"{urlPrefix}/decks{{id}}", (IDeckRepository deckRepo, int deckId) =>
                RemoveDeck(deckRepo, deckId))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);
        }


        private static IResult GetDeck(IDeckRepository deckRepo, int deckId, IMapper mapper)
        {
            try
            {
                Deck deck = deckRepo.GetDeck(deckId);
                DeckDetailDTO deckDetailDto = mapper.Map<Deck, DeckDetailDTO>(deck);
                return Results.Ok(deckDetailDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error getting deck: {ex.Message}");
            }
        }

        private static IResult GetAllDecks(IDeckRepository deckRepo, IMapper mapper)
        {
            try
            {
                IEnumerable<Deck> decks = deckRepo.GetAllDecks();
                List<DeckDTO> deckDTOs = mapper.Map<IEnumerable<Deck>, List<DeckDTO>>(decks);
                return Results.Ok(deckDTOs);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error getting decks: {ex.Message}");
            }
        }

        //private static IResult AddCardToDeck(IDeckRepository deckRepo, CardInDeckDTO cardInDeckDto, IMapper mapper, IConfiguration configuration)
        //{
        //    try
        //    {
        //        CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

        //        deckRepo.AddCardToDeck(cardInDeck, configuration.GetValue<int>("AppSettings:MaxCardsDeck"));

        //        return Results.Ok($"Card with id {cardInDeck.Id} with count {cardInDeck.Count} added to deck");
                

        //    }
        //    catch (Exception ex)
        //    {
        //        return Results.BadRequest($"Error adding card to deck: {ex.Message}");
        //    }
        //}


        private static IResult AddCardToDeck(IDeckRepository deckRepo, CardInDeckDTO cardInDeckDto , int deckId, IMapper mapper, IConfiguration configuration)
        {
            try
            {
                CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

                deckRepo.AddCardToDeck(deckId, cardInDeck);

                return Results.Ok($"Card with id {cardInDeck.Id} with count {cardInDeck.Count} added to deck");
                

            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error adding card to deck: {ex.Message}");
            }
        }


        private static IResult AddDeck(IDeckRepository deckRepo, DeckDTO deckDto, IMapper mapper)
        {
            try
            {
                Deck deck = mapper.Map<Deck>(deckDto);

                deckRepo.AddDeck(deck);

                return Results.Ok($"Deck with id {deck.Id} added");


            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error adding deck: {ex.Message}");
            }
        }

        private static IResult RemoveDeck(IDeckRepository deckRepo, int deckId)
        {
            try
            {
                deckRepo.RemoveDeck(deckId);
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
