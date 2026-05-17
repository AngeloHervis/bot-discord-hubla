using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Enums;
using bot_discord_hubla.Domain.Interfaces;
using bot_discord_hubla.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace bot_discord_hubla.Infrastructure.Repositories;

public class InscricaoRepository(AppDbContext context)
    : GenericRepository<Inscricao>(context), IInscricaoRepository
{
    public async Task<List<Inscricao>> ObterPendentesPorEmailAsync(string email, CancellationToken ct = default)
        => await DbSet
            .AsNoTracking()
            .Include(i => i.Produto)
            .Include(i => i.Membro)
            .Where(i => i.Membro!.Email == email.Trim().ToLowerInvariant()
                        && i.Status == StatusInscricao.Pendente)
            .ToListAsync(ct);

    public async Task<List<Inscricao>> ObterPendentesPorEmailTrackedAsync(string email, CancellationToken ct = default)
        => await DbSet
            .Include(i => i.Produto)
            .Include(i => i.Membro)
            .Where(i => i.Membro!.Email == email.Trim().ToLowerInvariant()
                        && i.Status == StatusInscricao.Pendente)
            .ToListAsync(ct);

    public async Task<Inscricao?> ObterAtivaOuPendentePorMembroEProdutoAsync(
        int membroId, int produtoId, CancellationToken ct = default)
        => await DbSet
            .FirstOrDefaultAsync(i =>
                i.MembroId == membroId &&
                i.ProdutoId == produtoId &&
                i.Status != StatusInscricao.Revogado, ct);

    public async Task<bool> PossuiOutraInscricaoAtivaAsync(
        int membroId, int inscricaoIdExcluir, CancellationToken ct = default)
        => await DbSet
            .AnyAsync(i =>
                i.MembroId == membroId &&
                i.Id != inscricaoIdExcluir &&
                i.Status == StatusInscricao.Ativado, ct);

    public async Task<List<string>> ObterCargosAtivosDoMembroAsync(int membroId, CancellationToken ct = default)
        => await DbSet
            .Where(i => i.MembroId == membroId && i.Status == StatusInscricao.Ativado)
            .Select(i => i.Produto!.RoleName)
            .Distinct()
            .ToListAsync(ct);
}
