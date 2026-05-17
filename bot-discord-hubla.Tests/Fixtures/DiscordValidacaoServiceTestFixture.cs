using bot_discord_hubla.Application.Interfaces;
using bot_discord_hubla.Application.Services;
using bot_discord_hubla.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace bot_discord_hubla.Tests.Fixtures;

public class DiscordValidacaoServiceTestFixture
{
    public Mock<IMembroRepository> MockMembroRepository { get; } = new();
    public Mock<IInscricaoRepository> MockInscricaoRepository { get; } = new();
    public Mock<IConfiguration> MockConfiguration { get; } = new();
    public Mock<ILogger<DiscordValidacaoService>> MockLogger { get; } = new();

    public IDiscordValidacaoService CreateService()
        => new DiscordValidacaoService(
            MockMembroRepository.Object,
            MockInscricaoRepository.Object,
            MockConfiguration.Object,
            MockLogger.Object);

    public void ResetMocks()
    {
        MockMembroRepository.Reset();
        MockInscricaoRepository.Reset();
        MockConfiguration.Reset();
        MockLogger.Reset();
    }
}
