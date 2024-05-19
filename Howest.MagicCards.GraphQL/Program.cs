

using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLAPI.GraphQLTypes;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<MTGContext>
    (options => options.UseSqlServer(config.GetConnectionString("MTG_Db")));

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();

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
