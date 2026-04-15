namespace NetShop;

public static class BlazorExtensions
{
    public static IServiceCollection AddAppBlazor(this IServiceCollection services)
    {
        services.AddRazorComponents().AddInteractiveServerComponents();

        return services;
    }
}
