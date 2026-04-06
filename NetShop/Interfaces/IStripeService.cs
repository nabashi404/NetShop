using FluentResults;
using NetShop.Data;
using Stripe.Checkout;

namespace NetShop.Interfaces;

public interface IStripeService
{
    Task<Result<Session>> CreateSessionAsync(ApplicationUser user, string stripePriceId);
}
