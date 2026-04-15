using FluentResults;
using NetShop.Models.Entities;

namespace NetShop.Interfaces;

public interface IOrderService
{
    Task<Result<Order>> CreatePendingAsync(string userId);
    Task<Result<Order>> GetAsync(long id);
    Task<Result> MarkAsPaidAsync(long orderId, string stripeSessionId);
    Task<Result> AttachStripeSessionAsync(long orderId, string sessionId);
}
