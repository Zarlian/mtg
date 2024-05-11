using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.OpenApi.Models;
using Type = System.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<MTGContext>
    (options => options.UseSqlServer(config.GetConnectionString("MTG_Db")));
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "MTG Card API version 1.1",
        Version = "v1.1",
        Description = "API to manage MTG cards"
    });
    c.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "MTG Card API version 1.5",
        Version = "v1.5",
        Description = "API to manage MGT cards"
    });
});


builder.Services.AddAutoMapper(new Type[] { typeof(Howest.MagicCards.Shared.Mappings.CardsProfile) });

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IRarityRepository, RarityRepository>();
builder.Services.AddScoped<ISetRepository, SetRepository>();

builder.Services.AddApiVersioning(o => {
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MTG API v1.1");
        c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "MTG API v1.5");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
