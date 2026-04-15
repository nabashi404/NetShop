using Stripe;

namespace NetShop.Interfaces;

public interface IPaymentService
{
    Task HandleStripeWebhookAsync(Event stripeEvent);
}
