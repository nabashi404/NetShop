using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetShop.Data;
using NetShop.Models;
using Stripe;
using Stripe.Checkout;

namespace NetShop.Endpoints;

public static class StripeEndpoints
{
    private static readonly PriceService _priceService = new();

    public static void MapStripeEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var route = endpoint.MapGroup("/api/stripe");

        route.MapPost("/webhook", Webhook);
    }

    static async Task<IResult> Webhook([FromServices] UserManager<ApplicationUser> userManager, [FromServices] ApplicationDbContext context, HttpRequest request, [FromServices] IOptions<StripeSettings> stripeSettings)
    {
        var requestContent = await new StreamReader(request.Body).ReadToEndAsync();

        var signature = request.Headers["Stripe-Signature"].ToString();

        var stripeEvent = EventUtility.ConstructEvent(requestContent, signature, stripeSettings.Value.WebhookSecretKey);

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session = stripeEvent.Data.Object as Session;

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.StripeCustomerId == session.CustomerId);

            var sessionLineItemService = new SessionLineItemService();

            var lineItems = sessionLineItemService.ListAutoPagingAsync(session.Id);

            await foreach (var item in lineItems)
            {
                var productId = long.Parse(item.Price.Product.ToString());

                var product = await context.Products.FindAsync(productId);

                if (product == null) continue;

                context.Orders.Add(new Models.Entities.Order());
            }

            await context.SaveChangesAsync();
        }

        return TypedResults.Ok();
    }
}
