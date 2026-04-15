using NetShop.Components;
using NetShop.Endpoints;

namespace NetShop;

public static class EndpointExtensions
{
    public static WebApplication MapAppEndpoints(this WebApplication app)
    {
        app.MapProductEndpoints();
        app.MapStripeEndpoints();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        return app;
    }
}
