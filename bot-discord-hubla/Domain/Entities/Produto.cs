namespace bot_discord_hubla.Domain.Entities;

public sealed class Produto
{
    public int Id { get; private set; }

    public string HublaProductId { get; private set; } = string.Empty;

    public string RoleName { get; private set; } = string.Empty;

    public Produto() { }

    public Produto(string hublaProductId, string roleName)
    {
        HublaProductId = hublaProductId;
        RoleName = roleName;
    }
}
