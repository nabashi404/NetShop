using Microsoft.AspNetCore.Components;
using NetShop.Enums;
using NetShop.Interfaces;
using NetShop.Models.Entities;

namespace NetShop.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private IProductService ProductService { get; set; } = default!;

    protected IEnumerable<Product> products = [];

    protected override async Task OnInitializedAsync()
    {
        var result = await ProductService.GetAllAsync();

        if (result.IsSuccess) products = result.Value.Where(p => p.Status == ProductStatus.Active);
    }
}
