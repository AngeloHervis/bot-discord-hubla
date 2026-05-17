using bot_discord_hubla.Application.Interfaces;
using Discord;
using Discord.WebSocket;

namespace bot_discord_hubla.Discord;

public class ValidarVipMessageHandler(
    DiscordSocketClient discordClient,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<ValidarVipMessageHandler> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        discordClient.MessageReceived += OnMessageReceived;

        var token = configuration["Discord:Token"]
            ?? throw new InvalidOperationException("Discord:Token não configurado em appsettings.");

        await discordClient.LoginAsync(TokenType.Bot, token);
        await discordClient.StartAsync();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.LogoutAsync();
        await discordClient.StopAsync();
        await base.StopAsync(cancellationToken);
    }

    private async Task OnMessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        var channelName = configuration["Discord:ValidarVipChannelName"] ?? "validar-vip";
        if (message.Channel.Name != channelName) return;

        using var scope = scopeFactory.CreateScope();
        var validacaoService = scope.ServiceProvider.GetRequiredService<IDiscordValidacaoService>();

        try
        {
            await validacaoService.ValidarEAtivarAsync(message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro no fluxo de validação VIP para discordId={DiscordId}", message.Author.Id);
        }
    }
}
