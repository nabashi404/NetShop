using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetShop.Data;
using NetShop.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace NetShop.Services;

public class PaymentService(UserManager<ApplicationUser> userManager, IOrderService orderService) : IPaymentService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IOrderService _orderService = orderService;

    public async Task HandleStripeWebhookAsync(Event stripeEvent)
    {
        if (stripeEvent.Type != "checkout.session.completed") return;

        var session = stripeEvent.Data.Object as Session;

        if (session == null) return;

        if (!session.Metadata.TryGetValue("orderId", out var orderIdValue)) return;

        if (!long.TryParse(orderIdValue, out var orderId)) return;

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.StripeCustomerId == session.CustomerId);

        if (user == null) return;

        await _orderService.MarkAsPaidAsync(orderId, session.Id);
    }
}
