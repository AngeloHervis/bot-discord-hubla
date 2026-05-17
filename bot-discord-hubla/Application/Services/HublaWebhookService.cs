using bot_discord_hubla.Application.Helpers;
using bot_discord_hubla.Application.Interfaces;
using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Interfaces;
using bot_discord_hubla.Shared.DataContracts.Requests;
using Discord;
using Discord.WebSocket;

namespace bot_discord_hubla.Application.Services;

public class HublaWebhookService(
    IMembroRepository membroRepository,
    IProdutoRepository produtoRepository,
    IInscricaoRepository inscricaoRepository,
    DiscordSocketClient discordClient,
    IConfiguration configuration,
    ILogger<HublaWebhookService> logger
) : IHublaWebhookService
{
    private const string MemberAdded = "customer.member_added";
    private const string MemberRemoved = "customer.member_removed";

    private static readonly SemaphoreSlim WebhookLock = new(1, 1);

    public async Task ProcessarAsync(HublaWebhookRequest request, CancellationToken ct = default)
    {
        var email = request.Event?.User?.Email?.Trim().ToLowerInvariant();
        var productId = request.Event?.Product?.Id;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(productId))
        {
            logger.LogWarning("Payload inválido — email ou productId ausente. Type={Type}", request.Type);
            return;
        }

        await WebhookLock.WaitAsync(ct);
        try
        {
            switch (request.Type)
            {
                case MemberAdded:
                    await ProcessarMemberAddedAsync(email, productId, ct);
                    break;
                case MemberRemoved:
                    await ProcessarMemberRemovedAsync(email, productId, ct);
                    break;
            }
        }
        finally
        {
            WebhookLock.Release();
        }
    }

    private async Task ProcessarMemberAddedAsync(string email, string hublaProductId, CancellationToken ct)
    {
        var produto = await ObterOuCriarProdutoAsync(hublaProductId, ct);
        var membro = await ObterOuCriarMembroAsync(email, ct);
        await UpsertInscricaoPendenteAsync(membro, produto, ct);
    }

    private async Task ProcessarMemberRemovedAsync(string email, string hublaProductId, CancellationToken ct)
    {
        var membro = await membroRepository.ObterPorEmailAsync(email, ct);
        if (membro is null)
        {
            logger.LogWarning("member_removed: membro não encontrado para email={Email}", email);
            return;
        }

        var produto = await produtoRepository.ObterPorHublaProductIdAsync(hublaProductId, ct);
        if (produto is null)
        {
            logger.LogWarning("member_removed: produto não encontrado. hublaProductId={ProdutoId}", hublaProductId);
            return;
        }

        var inscricao = await inscricaoRepository.ObterAtivaOuPendentePorMembroEProdutoAsync(membro.Id, produto.Id, ct);
        if (inscricao is null)
        {
            logger.LogWarning(
                "member_removed: inscrição não encontrada. membroId={MembroId}, produtoId={ProdutoId}",
                membro.Id, produto.Id);
            return;
        }

        inscricao.Revogar();
        await inscricaoRepository.SalvarAsync(ct);
        if (string.IsNullOrEmpty(membro.DiscordId))
            return;

        await SincronizarCargosDiscordAsync(membro, ct);
    }

    private async Task<Produto> ObterOuCriarProdutoAsync(string hublaProductId, CancellationToken ct)
    {
        var produto = await produtoRepository.ObterPorHublaProductIdAsync(hublaProductId, ct);
        if (produto is not null)
            return produto;

        var roleName = configuration[$"Hubla:ProdutoRoles:{hublaProductId}"];

        var novoProduto = new Produto(hublaProductId, roleName ?? hublaProductId);
        await produtoRepository.AdicionarESalvarAsync(novoProduto, ct);
        return novoProduto;
    }

    private async Task<Membro> ObterOuCriarMembroAsync(string email, CancellationToken ct)
    {
        var membro = await membroRepository.ObterPorEmailAsync(email, ct);
        if (membro is not null)
            return membro;

        var novoMembro = new Membro(email);
        await membroRepository.AdicionarESalvarAsync(novoMembro, ct);
        return novoMembro;
    }

    private async Task UpsertInscricaoPendenteAsync(Membro membro, Produto produto, CancellationToken ct)
    {
        var inscricao = await inscricaoRepository.ObterAtivaOuPendentePorMembroEProdutoAsync(membro.Id, produto.Id, ct);
        if (inscricao is not null)
        {
            inscricao.MarcarPendente();
            await inscricaoRepository.SalvarAsync(ct);
            return;
        }

        var novaInscricao = new Inscricao(membro.Id, produto.Id);
        await inscricaoRepository.AdicionarESalvarAsync(novaInscricao, ct);
    }

    private async Task SincronizarCargosDiscordAsync(Membro membro, CancellationToken ct)
    {
        var guildId = ulong.Parse(configuration["Discord:GuildId"]
            ?? throw new InvalidOperationException("Discord:GuildId não configurado."));

        var guild = discordClient.GetGuild(guildId);
        if (guild is null)
        {
            logger.LogError("Guild Discord não encontrada: guildId={GuildId}", guildId);
            return;
        }

        if (!ulong.TryParse(membro.DiscordId, out var discordUserId))
        {
            logger.LogError("DiscordId inválido no banco: discordId={DiscordId}", membro.DiscordId);
            return;
        }

        var guildUser = await ((IGuild)guild).GetUserAsync(discordUserId);
        if (guildUser is null)
        {
            logger.LogWarning("Usuário não encontrado na guild: discordId={DiscordId}", membro.DiscordId);
            return;
        }

        var cargosAtivos = await inscricaoRepository.ObterCargosAtivosDoMembroAsync(membro.Id, ct);
        var vipRoleName = configuration["Discord:VipRoleName"] ?? "aluno";
        var targetRole = CargoSincronizadorHelper.DeterminarTargetRole(cargosAtivos, vipRoleName);

        await CargoSincronizadorHelper.SincronizarAsync(guild.Roles, guildUser, targetRole, vipRoleName, logger);
    }
}
