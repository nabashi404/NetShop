namespace NetShop.Models;

public class StripeSettings
{
    public required string SecretKey { get; set; }
    public required string WebhookSecretKey { get; set; }
}
