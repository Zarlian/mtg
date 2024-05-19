using Howest.MagicCards.Web.Components;
using Howest.MagicCards.Web.Components.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("CardsAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7195/api/v1.5/");
});


builder.Services.AddHttpClient("DeckAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7079/api/");
});

builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<DeckService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
