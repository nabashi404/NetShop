using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NetShop.Interfaces;
using NetShop.Models.Entities;

namespace NetShop.Components.Pages.Dashboard;

public partial class Dashboard : ComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject] private IProductService ProductService { get; set; } = default!;

    protected List<Product> products = [];

    protected bool IsAllSelected
    {
        get => _isAllSelected;
        set
        {
            if (_isAllSelected == value)
            {
                return;
            }

            _isAllSelected = value;

            foreach (Product product in products)
            {
                product.IsSelected = _isAllSelected;
            }
        }
    }

    private bool _isAllSelected;

    protected override async Task OnInitializedAsync()
    {
        var result = await ProductService.GetAllAsync();

        if (result.IsSuccess) products = [.. result.Value];
    }

    protected async Task DeleteAsync(long id)
    {
        var product = products.FirstOrDefault(u => u.Id == id);

        if (product == null) return;

        var result = await ProductService.DeleteAsync(id);  

        if (result.IsSuccess)
        {
            products.Remove(product);
        }
        else
        {
            var message = result.Errors.FirstOrDefault()?.Message ?? "An error occurred.";

            await JSRuntime.InvokeVoidAsync("notify.showToast", "error", message);
        }
    }

    protected async Task DeleteSelectedAsync()
    {
        var selectedProducts = products.Where(p => p.IsSelected).ToArray();

        foreach (var product in selectedProducts)
        {
            var result = await ProductService.DeleteAsync(product.Id);

            if (result.IsSuccess)
            {
                products.Remove(product);
            }
            else
            {
                var message = result.Errors.FirstOrDefault()?.Message ?? "An error occurred.";

                await JSRuntime.InvokeVoidAsync("notify.showToast", "error", message);
            }
        }
    }

    protected void UpdateIsAllSelected()
    {
        _isAllSelected = products.All(p => p.IsSelected);
    }
}
