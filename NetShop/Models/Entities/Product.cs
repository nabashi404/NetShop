using NetShop.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetShop.Models.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Images { get; set; } = [];

    public long Price { get; set; }

    public Currency Currency { get; set; } = Currency.USD;

    public long StockQuantity { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

    [NotMapped] 
    public bool IsSelected { get; set; }

    public string? StripeProductId { get; set; }

    public string? StripePriceId { get; set; }
}
