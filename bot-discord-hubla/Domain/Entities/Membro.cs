namespace bot_discord_hubla.Domain.Entities;

public sealed class Membro
{
    public int Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? DiscordId { get; private set; }

    public ICollection<Inscricao> Inscricoes { get; init; } = [];

    public Membro() { }

    public Membro(string email)
    {
        Email = email.Trim().ToLowerInvariant();
    }

    public void VincularDiscordId(string discordId)
    {
        if (!string.IsNullOrEmpty(DiscordId) && DiscordId != discordId)
        {
            throw new InvalidOperationException("Este e-mail já está vinculado a outra conta do Discord.");
        }
        DiscordId = discordId;
    }
}
