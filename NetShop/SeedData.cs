using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetShop.Data;
using NetShop.Enums;
using Stripe;

namespace NetShop;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync();

        var user = await userManager.FindByEmailAsync("demo@demo.com");

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "demo@demo.com",
                Email = "demo@demo.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, "Demo123!");
        }

        if (context.Products.Any()) return;

        var priceService = new PriceService();
        var productService = new ProductService();

        var stripeIds = new[] { "price_1TIyxNAM2qpdJxnTCiK865FS", "price_1TIywwAM2qpdJxnTAp6MPQRS", "price_1TIyweAM2qpdJxnTqmRPTA0Z" };

        foreach (var stripeId in stripeIds)
        {
            var price = await priceService.GetAsync(stripeId);

            var stripeProduct = await productService.GetAsync(price.ProductId);

            var product = new Models.Entities.Product
            {
                Name = stripeProduct.Name,
                Description = stripeProduct.Description,
                Images = stripeProduct.Images,
                Price = price.UnitAmount ?? 0,
                StockQuantity = 0,
                Status = ProductStatus.Active,
                StripeProductId = stripeProduct.Id,
                StripePriceId = price.Id,
                CreatedAt = stripeProduct.Created,
                UpdatedAt = stripeProduct.Updated
            };

            context.Products.Add(product);
        }

        await context.SaveChangesAsync();
    }
}
