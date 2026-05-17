using bot_discord_hubla.Shared.DataContracts.Requests;

namespace bot_discord_hubla.Application.Interfaces;

public interface IHublaWebhookService
{
    Task ProcessarAsync(HublaWebhookRequest request, CancellationToken ct = default);
}
