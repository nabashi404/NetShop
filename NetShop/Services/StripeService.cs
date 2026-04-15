using FluentResults;
using Microsoft.AspNetCore.Identity;
using NetShop.Data;
using NetShop.Interfaces;
using NetShop.Models;
using Stripe;
using Stripe.Checkout;

namespace NetShop.Services;

public class StripeService(UserManager<ApplicationUser> userManager, IOrderService orderService, IProductService productService) : IStripeService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IOrderService _orderService = orderService;
    private readonly IProductService _productService = productService;

    public async Task<Result<Session>> CreateSessionAsync(ApplicationUser user, IEnumerable<CartItem> items, string baseUrl)
    {
        var customer = await GetCustomerAsync(user);

        var orderResult = await _orderService.CreatePendingAsync(user.Id);

        if (!orderResult.IsSuccess) return Result.Fail("Order creation failed");

        var order = orderResult.Value;

        var lineItems = new List<SessionLineItemOptions>();

        foreach (var item in items)
        {
            var productResult = await _productService.GetAsync(item.ProductId);

            if (!productResult.IsSuccess) continue;

            var product = productResult.Value;

            lineItems.Add(new SessionLineItemOptions
            {
                Price = product.StripePriceId,
                Quantity = item.Quantity
            });
        }

        if (lineItems.Count == 0) return Result.Fail("Cart is empty");

        var options = new SessionCreateOptions
        {
            Customer = customer.Id,
            PaymentMethodTypes = ["card"],
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = baseUrl,
            CancelUrl = baseUrl,
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id.ToString() }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        await _orderService.AttachStripeSessionAsync(order.Id, session.Id);

        return Result.Ok(session);
    }

    private async Task<Customer> GetCustomerAsync(ApplicationUser user)
    {
        var customerService = new CustomerService();

        if (string.IsNullOrEmpty(user.StripeCustomerId))
        {
            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = user.UserName
            });

            user.StripeCustomerId = customer.Id;

            await _userManager.UpdateAsync(user);

            return customer;
        }

        return await customerService.GetAsync(user.StripeCustomerId);
    }
}