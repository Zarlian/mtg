using AutoMapper;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.MinimalAPI.Mappings;
using Howest.MagicCards.Shared.Mappings;
using MongoDB.Driver;

const string commonPrefix = "/api";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IMongoClient>(new MongoClient(config.GetConnectionString("MongoDb")));
builder.Services.AddAutoMapper(typeof(CardsProfile));
builder.Services.AddScoped<IDeckRepository, DeckRepository>();

WebApplication app = builder.Build();
string urlPrefix = config.GetSection("ApiPrefix").Value ?? commonPrefix;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDeckEndpoints(urlPrefix, app.Services.GetRequiredService<IMapper>(), config);

app.Run();

