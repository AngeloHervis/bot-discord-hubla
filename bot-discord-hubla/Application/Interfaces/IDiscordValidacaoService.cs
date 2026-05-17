using Discord.WebSocket;

namespace bot_discord_hubla.Application.Interfaces;

public interface IDiscordValidacaoService
{
    Task ValidarEAtivarAsync(SocketMessage message, CancellationToken ct = default);
}
