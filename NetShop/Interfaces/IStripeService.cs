using FluentResults;
using NetShop.Data;
using NetShop.Models;
using Stripe.Checkout;

namespace NetShop.Interfaces;

public interface IStripeService
{
    Task<Result<Session>> CreateSessionAsync(ApplicationUser user, IEnumerable<CartItem> items, string baseUrl);
}
