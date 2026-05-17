using bot_discord_hubla.Domain.Entities;
using FluentAssertions;

namespace bot_discord_hubla.Tests.Domain;

public sealed class MembroTest
{
    [Fact]
    public void VincularDiscordId_QuandoJaVinculadoAOutro_DeveLancarExcecao()
    {
        var membro = new Membro("teste@teste.com");
        membro.VincularDiscordId("12345");

        var action = () => membro.VincularDiscordId("67890");

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Este e-mail já está vinculado a outra conta do Discord.");
    }

    [Fact]
    public void VincularDiscordId_QuandoMesmoId_DevePermitir()
    {
        var membro = new Membro("teste@teste.com");
        membro.VincularDiscordId("12345");

        var action = () => membro.VincularDiscordId("12345");

        action.Should().NotThrow();
    }
}
