using FluentResults;
using Microsoft.AspNetCore.Identity;
using NetShop.Data;
using NetShop.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace NetShop.Services;

public class StripeService(UserManager<ApplicationUser> userManager) : IStripeService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<Session>> CreateSessionAsync(ApplicationUser user, string stripePriceId)
    {
        var customerService = new CustomerService();

        var customer = await customerService.CreateAsync(new CustomerCreateOptions
        {
            Email = user.UserName
        });

        user.StripeCustomerId = customer.Id;

        await _userManager.UpdateAsync(user);

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = ["card"],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = stripePriceId,
                    Quantity = 1,
                },
            ],
            Mode = "payment",
            SuccessUrl = "http://localhost:8000/success",
            CancelUrl = "http://localhost:8000/cancel"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return Result.Ok(session);
    }
}