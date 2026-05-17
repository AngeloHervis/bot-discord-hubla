using bot_discord_hubla.Application.Interfaces;
using bot_discord_hubla.Application.Services;
using bot_discord_hubla.Domain.Interfaces;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace bot_discord_hubla.Tests.Fixtures;

public class HublaWebhookServiceTestFixture
{
    public Mock<IMembroRepository> MockMembroRepository { get; } = new();
    public Mock<IProdutoRepository> MockProdutoRepository { get; } = new();
    public Mock<IInscricaoRepository> MockInscricaoRepository { get; } = new();
    public Mock<DiscordSocketClient> MockDiscordClient { get; } = new();
    public Mock<IConfiguration> MockConfiguration { get; } = new();
    public Mock<ILogger<HublaWebhookService>> MockLogger { get; } = new();

    public IHublaWebhookService CreateService()
        => new HublaWebhookService(
            MockMembroRepository.Object,
            MockProdutoRepository.Object,
            MockInscricaoRepository.Object,
            MockDiscordClient.Object,
            MockConfiguration.Object,
            MockLogger.Object);

    public void ResetMocks()
    {
        MockMembroRepository.Reset();
        MockProdutoRepository.Reset();
        MockInscricaoRepository.Reset();
        MockDiscordClient.Reset();
        MockConfiguration.Reset();
        MockLogger.Reset();
    }
}
