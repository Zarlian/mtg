using GraphQL.Types;
using Howest.MagicCards.GraphQL.Queries;

namespace GraphQLAPI.GraphQLTypes
{
    public class RootSchema : Schema
    {
        public RootSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
        }
    }
}
