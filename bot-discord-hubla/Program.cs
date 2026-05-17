using bot_discord_hubla.API.Filters;
using bot_discord_hubla.Application.Interfaces;
using bot_discord_hubla.Application.Services;
using bot_discord_hubla.Discord;
using bot_discord_hubla.Domain.Interfaces;
using bot_discord_hubla.Infrastructure.Data;
using bot_discord_hubla.Infrastructure.Repositories;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMembroRepository, MembroRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IInscricaoRepository, InscricaoRepository>();

builder.Services.AddScoped<IHublaWebhookService, HublaWebhookService>();
builder.Services.AddScoped<IDiscordValidacaoService, DiscordValidacaoService>();

builder.Services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.Guilds
                   | GatewayIntents.GuildMessages
                   | GatewayIntents.MessageContent
                   | GatewayIntents.GuildMembers,
    LogLevel = LogSeverity.Info
}));

builder.Services.AddHostedService<ValidarVipMessageHandler>();

builder.Services.AddScoped<HublaTokenAuthFilter>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseRouting();
app.MapControllers();

await app.RunAsync();