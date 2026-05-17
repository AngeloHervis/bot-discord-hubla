using System.Text.RegularExpressions;
using bot_discord_hubla.Application.Helpers;
using bot_discord_hubla.Application.Interfaces;
using bot_discord_hubla.Domain.Interfaces;
using Discord;
using Discord.WebSocket;

namespace bot_discord_hubla.Application.Services;

public partial class DiscordValidacaoService(
    IMembroRepository membroRepository,
    IInscricaoRepository inscricaoRepository,
    IConfiguration configuration,
    ILogger<DiscordValidacaoService> logger
) : IDiscordValidacaoService
{
    [GeneratedRegex(@"[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}")]
    private static partial Regex EmailRegex();

    private static readonly SemaphoreSlim ValidacaoLock = new(1, 1);

    public async Task ValidarEAtivarAsync(SocketMessage message, CancellationToken ct = default)
    {
        try { await message.DeleteAsync(); }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Não foi possível deletar mensagem do discordId={DiscordId}", message.Author.Id);
        }

        var email = ExtrairEmail(message.Content);
        if (email is null)
        {
            await EnviarMensagemAsync(message, "❌ Não consegui identificar um e-mail válido na sua mensagem. Tente novamente.");
            return;
        }

        await ValidacaoLock.WaitAsync(ct);
        try
        {
            var inscricoesPendentes = await inscricaoRepository.ObterPendentesPorEmailTrackedAsync(email, ct);
            if (inscricoesPendentes.Count == 0)
            {
                await EnviarMensagemAsync(message,
                    "❌ Não encontrei nenhuma compra pendente vinculada ao e-mail informado.\n" +
                    "Verifique se usou o mesmo e-mail da compra na Hubla ou aguarde alguns minutos.");
                return;
            }

            var membroId = inscricoesPendentes[0].MembroId;
            var membro = await membroRepository.ObterMembroTrackedAsync(membroId, ct);
            if (membro is null)
            {
                logger.LogError("Inconsistência: membro não encontrado no banco. membroId={MembroId}", membroId);
                return;
            }

            try
            {
                membro.VincularDiscordId(message.Author.Id.ToString());
            }
            catch (InvalidOperationException)
            {
                await EnviarMensagemAsync(message,
                    "⚠️ Este e-mail já está vinculado a outra conta do Discord. Se acha que é um erro, contate o suporte.");
                return;
            }

            var guildUser = await ObterGuildUserAsync(message);
            if (guildUser is null)
                return;

            var guild = ((SocketGuildChannel)message.Channel).Guild;

            foreach (var inscricao in inscricoesPendentes)
                inscricao.Ativar();

            await inscricaoRepository.SalvarAsync(ct);

            var cargosAtivos = await inscricaoRepository.ObterCargosAtivosDoMembroAsync(membro.Id, ct);
            var vipRoleName = configuration["Discord:VipRoleName"] ?? "aluno";
            var targetRole = CargoSincronizadorHelper.DeterminarTargetRole(cargosAtivos, vipRoleName);

            await CargoSincronizadorHelper.SincronizarAsync(guild.Roles, guildUser, targetRole, vipRoleName, logger);

            await EnviarMensagemAsync(message,
                $"✅ **Acesso liberado com sucesso!**\n" +
                $"Cargo atribuído: **{targetRole ?? "nenhum"}**\n\n" +
                $"Bem-vindo(a)! 🎉");
        }
        finally
        {
            ValidacaoLock.Release();
        }
    }

    private static string? ExtrairEmail(string conteudo)
    {
        var match = EmailRegex().Match(conteudo);
        return match.Success ? match.Value.Trim().ToLowerInvariant() : null;
    }

    private async Task<IGuildUser?> ObterGuildUserAsync(SocketMessage message)
    {
        if (message.Channel is not SocketGuildChannel guildChannel)
        {
            logger.LogWarning("Mensagem recebida fora de um canal de guild. discordId={DiscordId}", message.Author.Id);
            return null;
        }

        var guildUser = await ((IGuild)guildChannel.Guild).GetUserAsync(message.Author.Id);
        if (guildUser is null)
            logger.LogWarning("Usuário não encontrado na guild: discordId={DiscordId}", message.Author.Id);

        return guildUser;
    }

    private static async Task EnviarMensagemAsync(SocketMessage originalMessage, string mensagem)
    {
        try
        {
            var dm = await originalMessage.Author.CreateDMChannelAsync();
            await dm.SendMessageAsync(mensagem);
        }
        catch
        {
            var fallbackMsg = await originalMessage.Channel.SendMessageAsync(
                $"<@{originalMessage.Author.Id}> {mensagem}");
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                try { await fallbackMsg.DeleteAsync(); }
                catch
                {
                    // ignored
                }
            });
        }
    }
}
