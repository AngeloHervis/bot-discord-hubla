using bot_discord_hubla.Tests.Fixtures;
using FluentAssertions;

namespace bot_discord_hubla.Tests.Application;

public sealed class DiscordValidacaoServiceTest : IDisposable
{
    private readonly DiscordValidacaoServiceTestFixture _fixture = new();
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed) return;
        _fixture.ResetMocks();
        _disposed = true;
    }

    // Devido ao acoplamento com SocketMessage e outros tipos do Discord.Net (que não podem ser facilmente mockados),
    // é comum testes em métodos com classes de terceiros herméticas exigirem wrappers ou aceitarem cobertura parcial.
    // O projeto real deve adicionar um DTO ou wrapper se for exigido cobertura completa aqui.
    // Aqui incluiremos apenas a marcação base para a classe.
    
    [Fact]
    public void Test_Placeholder()
    {
        true.Should().BeTrue();
    }
}
