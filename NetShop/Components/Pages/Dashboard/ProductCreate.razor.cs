using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NetShop.Enums;
using NetShop.Interfaces;
using NetShop.Models.Dtos;

namespace NetShop.Components.Pages.Dashboard;

public partial class ProductCreate : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Inject] private IProductService ProductService { get; set; } = default!;

    [Inject] private IUploadService UploadService { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected ProductDto ProductDto = new() { Price = 1, StockQuantity = 1, Status = ProductStatus.Active};

    protected async Task HandleSelected(InputFileChangeEventArgs inputFile)
    {
        var file = inputFile.File;

        var result = await UploadService.UploadImageAsync(file);

        if (result.IsSuccess && result.Value != null)
        {
            ProductDto.Images = [result.Value.Url];
        }
    }

    protected async Task CreateAsync()
    {
        var result = await ProductService.CreateAsync(ProductDto);

        if (result.IsSuccess)
        {
            Navigation.NavigateTo("/dashboard");
        }
        else
        {
            var message = result.Errors.FirstOrDefault()?.Message ?? "An error occurred.";

            await JSRuntime.InvokeVoidAsync("notify.showToast", "error", message);
        }
    }
}
