using bot_discord_hubla.Domain.Entities;

namespace bot_discord_hubla.Tests.TestBuilders;

public class MembroBuilder : BaseBuilder<Membro>
{
    public MembroBuilder()
    {
        Faker.RuleFor(m => m.Id, f => f.Random.Int(1, 1000));
        Faker.RuleFor(m => m.Email, f => f.Internet.Email());
        Faker.RuleFor(m => m.DiscordId, f => f.Random.ULong().ToString());
    }

    public MembroBuilder ComEmail(string email)
    {
        Faker.RuleFor(m => m.Email, email);
        return this;
    }

    public MembroBuilder ComDiscordId(string? discordId)
    {
        Faker.RuleFor(m => m.DiscordId, discordId);
        return this;
    }
}
