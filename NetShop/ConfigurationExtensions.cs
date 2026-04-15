using NetShop.Models;
using Stripe;

namespace NetShop;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var stripeSection = config.GetSection("Stripe");
        var stripeSettings = stripeSection.Get<StripeSettings>() ?? throw new InvalidOperationException("Stripe settings are missing.");

        services.Configure<StripeSettings>(stripeSection); 

        StripeConfiguration.ApiKey = stripeSettings.SecretKey;

        var jwtSection = config.GetSection("JWT");
        var jwtSettings = jwtSection.Get<JWTSettings>() ?? throw new InvalidOperationException("JWT settings are missing.");

        services.Configure<JWTSettings>(jwtSection);

        return services;
    }
}
