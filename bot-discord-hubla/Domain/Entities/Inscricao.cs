using bot_discord_hubla.Domain.Enums;

namespace bot_discord_hubla.Domain.Entities;

public sealed class Inscricao
{
    public int Id { get; private set; }
    public int MembroId { get; init; }
    public int ProdutoId { get; init; }
    public StatusInscricao Status { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Membro? Membro { get; init; }
    public Produto? Produto { get; init; }

    public Inscricao() { }

    public Inscricao(int membroId, int produtoId)
    {
        MembroId = membroId;
        ProdutoId = produtoId;
        Status = StatusInscricao.Pendente;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Status = StatusInscricao.Ativado;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Revogar()
    {
        Status = StatusInscricao.Revogado;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarPendente()
    {
        Status = StatusInscricao.Pendente;
        UpdatedAt = DateTime.UtcNow;
    }
}
