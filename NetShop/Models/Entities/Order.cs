namespace NetShop.Models.Entities;

public class Order : BaseEntity
{
    public string UserId { get; set; }

    public string? StripeSessionId { get; set; }

    public ICollection<OrderItem> Items { get; set; } = [];
}
