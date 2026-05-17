using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Enums;

namespace bot_discord_hubla.Tests.TestBuilders;

public class InscricaoBuilder : BaseBuilder<Inscricao>
{
    public InscricaoBuilder()
    {
        Faker.RuleFor(i => i.Id, f => f.Random.Int(1, 1000));
        Faker.RuleFor(i => i.MembroId, f => f.Random.Int(1, 1000));
        Faker.RuleFor(i => i.ProdutoId, f => f.Random.Int(1, 1000));
        Faker.RuleFor(i => i.Status, f => f.PickRandom<StatusInscricao>());
        Faker.RuleFor(i => i.UpdatedAt, f => f.Date.Recent());
    }

    public InscricaoBuilder ComStatus(StatusInscricao status)
    {
        Faker.RuleFor(i => i.Status, status);
        return this;
    }

    public InscricaoBuilder ComMembroEProduto(Membro membro, Produto produto)
    {
        Faker.RuleFor(i => i.MembroId, membro.Id);
        Faker.RuleFor(i => i.Membro, membro);
        Faker.RuleFor(i => i.ProdutoId, produto.Id);
        Faker.RuleFor(i => i.Produto, produto);
        return this;
    }
}
