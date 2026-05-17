using bot_discord_hubla.Domain.Entities;

namespace bot_discord_hubla.Domain.Interfaces;

public interface IInscricaoRepository : IGenericRepository<Inscricao>
{
    Task<List<Inscricao>> ObterPendentesPorEmailAsync(string email, CancellationToken ct = default);

    Task<List<Inscricao>> ObterPendentesPorEmailTrackedAsync(string email, CancellationToken ct = default);

    Task<Inscricao?> ObterAtivaOuPendentePorMembroEProdutoAsync(int membroId, int produtoId, CancellationToken ct = default);

    Task<bool> PossuiOutraInscricaoAtivaAsync(int membroId, int inscricaoIdExcluir, CancellationToken ct = default);

    Task<List<string>> ObterCargosAtivosDoMembroAsync(int membroId, CancellationToken ct = default);
}
