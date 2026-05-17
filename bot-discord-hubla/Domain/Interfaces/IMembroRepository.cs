using bot_discord_hubla.Domain.Entities;

namespace bot_discord_hubla.Domain.Interfaces;

public interface IMembroRepository : IGenericRepository<Membro>
{
    Task<Membro?> ObterPorEmailAsync(string email, CancellationToken ct = default);

    Task<Membro?> ObterMembroTrackedAsync(int membroId, CancellationToken ct = default);
}

