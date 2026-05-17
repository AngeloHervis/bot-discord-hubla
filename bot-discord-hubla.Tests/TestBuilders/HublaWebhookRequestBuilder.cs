using bot_discord_hubla.Shared.DataContracts.Requests;

namespace bot_discord_hubla.Tests.TestBuilders;

public class HublaWebhookRequestBuilder : BaseBuilder<HublaWebhookRequest>
{
    public HublaWebhookRequestBuilder()
    {
        Faker.RuleFor(r => r.Type, f => f.PickRandom("customer.member_added", "customer.member_removed"));
        Faker.RuleFor(r => r.Version, "2.0");
        Faker.RuleFor(r => r.Event, f => new HublaEventData
        {
            User = new HublaUser
            {
                Email = f.Internet.Email(),
                FirstName = f.Name.FirstName(),
                LastName = f.Name.LastName(),
                Id = f.Random.AlphaNumeric(10)
            },
            Product = new HublaProduct
            {
                Id = f.Random.AlphaNumeric(10),
                Name = f.Commerce.ProductName()
            }
        });
    }

    public HublaWebhookRequestBuilder ComTipo(string type)
    {
        Faker.RuleFor(r => r.Type, type);
        return this;
    }

    public HublaWebhookRequestBuilder ComEmail(string? email)
    {
        Faker.FinishWith((f, r) => 
        {
            if (r.Event?.User != null)
                r.Event.User.Email = email!;
        });
        return this;
    }

    public HublaWebhookRequestBuilder ComProdutoId(string? productId)
    {
        Faker.FinishWith((f, r) => 
        {
            if (r.Event?.Product != null)
                r.Event.Product.Id = productId!;
        });
        return this;
    }
}
