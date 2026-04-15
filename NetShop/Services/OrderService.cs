using FluentResults;
using Microsoft.EntityFrameworkCore;
using NetShop.Data;
using NetShop.Interfaces;
using Order = NetShop.Models.Entities.Order;

namespace NetShop.Services;

public class OrderService(ApplicationDbContext context) : IOrderService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Order>> CreatePendingAsync(string userId)
    {
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Result.Ok(order);
    }

    public async Task<Result<Order>> GetAsync(long id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return Result.Fail("Order not found");

        return Result.Ok(order);
    }

    public async Task<Result> MarkAsPaidAsync(long orderId, string stripeSessionId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
            return Result.Fail("Order not found");

        await _context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> AttachStripeSessionAsync(long orderId, string sessionId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null) return Result.Fail("Order not found");

        order.StripeSessionId = sessionId;

        await _context.SaveChangesAsync();

        return Result.Ok();
    }
}

