using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Interfaces;
using bot_discord_hubla.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace bot_discord_hubla.Infrastructure.Repositories;

public class ProdutoRepository(AppDbContext context)
    : GenericRepository<Produto>(context), IProdutoRepository
{
    public async Task<Produto?> ObterPorHublaProductIdAsync(string hublaProductId, CancellationToken ct = default)
        => await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.HublaProductId == hublaProductId, ct);
}
