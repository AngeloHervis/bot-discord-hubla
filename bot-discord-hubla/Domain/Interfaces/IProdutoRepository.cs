using bot_discord_hubla.Domain.Entities;

namespace bot_discord_hubla.Domain.Interfaces;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    Task<Produto?> ObterPorHublaProductIdAsync(string hublaProductId, CancellationToken ct = default);
}
