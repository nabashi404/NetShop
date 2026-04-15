using NetShop.Interfaces;
using NetShop.Services;
using NetShop.Services.JsInterop;

namespace NetShop;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<IUploadService, UploadService>();

        services.AddScoped<Toastify>();

        return services;
    }
}
