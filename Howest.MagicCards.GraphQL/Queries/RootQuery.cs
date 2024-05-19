using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using GraphQL;
using Howest.MagicCards.DAL.Models;

namespace Howest.MagicCards.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepo, IArtistRepository artistRepo) 
        {
            Name = "MGT";

             FieldAsync<ListGraphType<Types.CardType>>(
                name: "cards",
                arguments: new QueryArguments
                {
                    new QueryArgument<StringGraphType> { Name = "power"},
                    new QueryArgument<StringGraphType> { Name = "toughness"}
                },
                resolve: async context =>
                {
                    string power = context.GetArgument<string>("power");
                    string toughness = context.GetArgument<string>("toughness");

                    return await CardQuery.GetCardsAsync(cardRepo, power, toughness);
                }
                );

            FieldAsync<ListGraphType<Types.ArtistType>>(
                name: "artists",
                resolve: async context =>
                {
                    IQueryable<Artist> artists = await artistRepo.GetAllArtistsAsync();
                    return artists.ToList();
                }
                );
        }
    }
}
