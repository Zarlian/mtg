using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using GraphQL;

namespace Howest.MagicCards.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepo) 
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

                    return await cardRepo.GetAllCardsAsync();
                }
                );
        }
    }
}
