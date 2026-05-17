using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Tests.Fixtures;
using bot_discord_hubla.Tests.TestBuilders;
using Moq;

namespace bot_discord_hubla.Tests.Application;

public sealed class HublaWebhookServiceTest : IDisposable
{
    private readonly HublaWebhookServiceTestFixture _fixture = new();
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed) return;
        _fixture.ResetMocks();
        _disposed = true;
    }

    [Fact]
    public async Task ProcessarAsync_QuandoEmailVazio_DeveRetornarSemProcessar()
    {
        var request = new HublaWebhookRequestBuilder().ComEmail(null).Build();
        var service = _fixture.CreateService();

        await service.ProcessarAsync(request, CancellationToken.None);

        _fixture.MockMembroRepository.Verify(r => r.ObterPorEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessarMemberAddedAsync_QuandoProdutoNaoExiste_DeveCriarProdutoComFallback()
    {
        var request = new HublaWebhookRequestBuilder().ComTipo("customer.member_added").Build();
        
        _fixture.MockProdutoRepository
            .Setup(r => r.ObterPorHublaProductIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Produto?)null);

        _fixture.MockConfiguration
            .Setup(c => c[$"Hubla:ProdutoRoles:{request.Event.Product.Id}"])
            .Returns((string?)null);

        var service = _fixture.CreateService();

        await service.ProcessarAsync(request, CancellationToken.None);

        _fixture.MockProdutoRepository.Verify(
            r => r.AdicionarESalvarAsync(It.Is<Produto>(p => p.HublaProductId == request.Event.Product.Id && p.RoleName == request.Event.Product.Id), It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task ProcessarMemberRemovedAsync_QuandoMembroNaoEncontrado_DeveRetornarSemErro()
    {
        var request = new HublaWebhookRequestBuilder().ComTipo("customer.member_removed").Build();
        
        _fixture.MockMembroRepository
            .Setup(r => r.ObterPorEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Membro?)null);

        var service = _fixture.CreateService();

        await service.ProcessarAsync(request, CancellationToken.None);

        _fixture.MockProdutoRepository.Verify(r => r.ObterPorHublaProductIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
