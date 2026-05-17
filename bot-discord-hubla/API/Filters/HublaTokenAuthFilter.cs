using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;
using System.Text;

namespace bot_discord_hubla.API.Filters;

/// <summary>
/// Valida o header x-hubla-token em todas as actions decoradas.
/// Usa comparação de tempo constante para evitar timing attacks.
/// </summary>
public class HublaTokenAuthFilter(IConfiguration configuration, ILogger<HublaTokenAuthFilter> logger)
    : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var tokenEsperado = configuration["Hubla:WebhookToken"];

        if (string.IsNullOrWhiteSpace(tokenEsperado))
        {
            logger.LogError("Hubla:WebhookToken não configurado em appsettings.");
            context.Result = new StatusCodeResult(500);
            return;
        }

        context.HttpContext.Request.Headers.TryGetValue("x-hubla-token", out var tokenRecebido);

        var bytesRecebido = Encoding.UTF8.GetBytes(tokenRecebido.ToString());
        var bytesEsperado = Encoding.UTF8.GetBytes(tokenEsperado);

        // Garante buffers de mesmo tamanho para evitar timing oracle de comprimento
        var bufferRecebido = new byte[bytesEsperado.Length];
        var copyLen = Math.Min(bytesRecebido.Length, bufferRecebido.Length);
        bytesRecebido.AsSpan(0, copyLen).CopyTo(bufferRecebido);

        var valido = CryptographicOperations.FixedTimeEquals(bufferRecebido, bytesEsperado)
                     && bytesRecebido.Length == bytesEsperado.Length;

        if (valido)
            return;

        logger.LogWarning("Webhook recebido com x-hubla-token inválido.");
        context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
