using bot_discord_hubla.API.Filters;
using bot_discord_hubla.Application.Interfaces;
using bot_discord_hubla.Shared.DataContracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace bot_discord_hubla.API.Controllers;

[ApiController]
[Route("webhook")]
public class WebhookController(IHublaWebhookService webhookService) : ControllerBase
{
    [HttpPost("hubla")]
    [ServiceFilter(typeof(HublaTokenAuthFilter))]
    public async Task<IActionResult> Hubla([FromBody] HublaWebhookRequest request, CancellationToken ct)
    {
        await webhookService.ProcessarAsync(request, ct);
        return Ok();
    }
}
