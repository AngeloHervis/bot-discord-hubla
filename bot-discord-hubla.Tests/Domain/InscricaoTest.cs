using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Enums;
using FluentAssertions;

namespace bot_discord_hubla.Tests.Domain;

public sealed class InscricaoTest
{
    [Fact]
    public void Ativar_DeveAlterarStatusEAtualizarTimestamp()
    {
        var inscricao = new Inscricao(1, 1);
        var oldDate = inscricao.UpdatedAt;

        Task.Delay(10).WaitAsync(CancellationToken.None);

        inscricao.Ativar();

        inscricao.Status.Should().Be(StatusInscricao.Ativado);
        inscricao.UpdatedAt.Should().BeAfter(oldDate);
    }

    [Fact]
    public void Revogar_DeveAlterarStatusEAtualizarTimestamp()
    {
        var inscricao = new Inscricao(1, 1);
        var oldDate = inscricao.UpdatedAt;

        Task.Delay(10).WaitAsync(CancellationToken.None);

        inscricao.Revogar();

        inscricao.Status.Should().Be(StatusInscricao.Revogado);
        inscricao.UpdatedAt.Should().BeAfter(oldDate);
    }
}
