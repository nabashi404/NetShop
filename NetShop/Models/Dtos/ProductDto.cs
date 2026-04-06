using NetShop.Enums;

namespace NetShop.Models.Dtos;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> Images { get; set; } = [];

    public decimal Price { get; set; } = 0m;

    public Currency Currency { get; set; } = Currency.USD;

    public long StockQuantity { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
