

using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLAPI.GraphQLTypes;
using Howest.MagicCards.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddScoped<RootSchema>();
builder.Services.AddGraphQL()
                .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
                .AddDataLoader()
                .AddSystemTextJson();

var app = builder.Build();

app.UseGraphQL<RootSchema>();
app.UseGraphQLPlayground(
    "/ui/playground",
    new PlaygroundOptions()
    {
        EditorTheme = EditorTheme.Dark
    });

app.Run();
