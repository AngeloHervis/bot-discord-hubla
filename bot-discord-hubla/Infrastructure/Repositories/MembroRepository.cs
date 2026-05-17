using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Interfaces;
using bot_discord_hubla.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace bot_discord_hubla.Infrastructure.Repositories;

public class MembroRepository(AppDbContext context)
    : GenericRepository<Membro>(context), IMembroRepository
{
    public async Task<Membro?> ObterPorEmailAsync(string email, CancellationToken ct = default)
        => await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Email == email.Trim().ToLowerInvariant(), ct);

    public async Task<Membro?> ObterComInscricoesAsync(string email, CancellationToken ct = default)
        => await DbSet
            .Include(m => m.Inscricoes)
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(m => m.Email == email.Trim().ToLowerInvariant(), ct);

    public async Task<Membro?> ObterMembroTrackedAsync(int membroId, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(m => m.Id == membroId, ct);
}

