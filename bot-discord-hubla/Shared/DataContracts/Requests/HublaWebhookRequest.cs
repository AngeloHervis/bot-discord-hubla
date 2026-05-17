using System.Text.Json.Serialization;

namespace bot_discord_hubla.Shared.DataContracts.Requests;

public class HublaWebhookRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("event")]
    public HublaEventData Event { get; set; } = new();
}

public class HublaEventData
{
    [JsonPropertyName("product")]
    public HublaProduct Product { get; set; } = new();

    [JsonPropertyName("user")]
    public HublaUser User { get; set; } = new();

    [JsonPropertyName("subscription")]
    public HublaSubscription? Subscription { get; set; }
}

public class HublaProduct
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class HublaUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("document")]
    public string? Document { get; set; }
}

public class HublaSubscription
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
