using Microsoft.AspNetCore.Components;
using NetShop.Interfaces;
using NetShop.Models.Entities;

namespace NetShop.Components.Pages;

public partial class ProductDetails : ComponentBase
{
    [Inject] public required IProductService ProductService { get; set; }

    [Parameter] public long Id { get; set; }

    protected Product product = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await ProductService.GetAsync(Id);

        if (result.IsSuccess && result.Value != null) product = result.Value;
    }
}
