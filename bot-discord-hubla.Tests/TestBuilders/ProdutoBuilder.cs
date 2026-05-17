using bot_discord_hubla.Domain.Entities;

namespace bot_discord_hubla.Tests.TestBuilders;

public class ProdutoBuilder : BaseBuilder<Produto>
{
    public ProdutoBuilder()
    {
        Faker.RuleFor(p => p.Id, f => f.Random.Int(1, 1000));
        Faker.RuleFor(p => p.HublaProductId, f => f.Random.AlphaNumeric(10));
        Faker.RuleFor(p => p.RoleName, f => f.Commerce.Department());
    }

    public ProdutoBuilder ComRoleName(string roleName)
    {
        Faker.RuleFor(p => p.RoleName, roleName);
        return this;
    }
}
