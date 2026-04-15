using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetShop.Interfaces;
using NetShop.Models;
using Stripe;

namespace NetShop.Endpoints;

public static class StripeEndpoints
{
    public static void MapStripeEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var route = endpoint.MapGroup("/api/stripe");

        route.MapPost("/webhook", Webhook);
    }

    static async Task<IResult> Webhook([FromServices] IPaymentService paymentService, HttpRequest request, [FromServices] IOptions<StripeSettings> stripeSettings)
    {
        using var reader = new StreamReader(request.Body);
        var requestContent = await reader.ReadToEndAsync();

        if (!request.Headers.TryGetValue("Stripe-Signature", out var signature)) return Results.BadRequest();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(requestContent, signature, stripeSettings.Value.WebhookSecretKey);

            await paymentService.HandleStripeWebhookAsync(stripeEvent);
        }
        catch (StripeException error)
        {
            return Results.BadRequest(error.Message);
        }
        catch
        {
            return Results.StatusCode(500);
        }

        return TypedResults.Ok();
    }
}
