using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetShop.Enums;
using NetShop.Interfaces;
using NetShop.Models.Dtos;
using NetShop.Services.JsInterop;

namespace NetShop.Components.Pages.Dashboard.Products;

public partial class ProductForm : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Inject] private IProductService ProductService { get; set; } = default!;

    [Inject] private IUploadService UploadService { get; set; } = default!;

    [Inject] private Toastify Toastify { get; set; } = default!;

    [Parameter] public long? Id { get; set; }

    protected ProductDto ProductDto = new() { Price = 1, StockQuantity = 1, Status = ProductStatus.Active };

    protected override async Task OnInitializedAsync()
    {
        if (Id.HasValue)
        {
            var result = await ProductService.GetAsync(Id.Value);

            if (result.IsSuccess)
            {
                ProductDto = new ProductDto
                {
                    Name = result.Value.Name,
                    Price = result.Value.Price
                };
            }
        }
    }

    protected async Task HandleSelected(InputFileChangeEventArgs inputFile)
    {
        var file = inputFile.File;

        var result = await UploadService.UploadImageAsync(file);

        if (result.IsSuccess && result.Value != null)
        {
            ProductDto.Images = [result.Value.Url];
        }
    }

    private async Task SaveAsync()
    {
        if (Id.HasValue)
        {
            var result = await ProductService.UpdateAsync(Id.Value, ProductDto);

            if (result.IsSuccess)
            {
                Navigation.NavigateTo("/dashboard");
            }
            else
            {
                var message = result.Errors.FirstOrDefault()?.Message ?? "An error occurred.";

                //await JSRuntime.InvokeVoidAsync("notify.showToast", "error", message);
                await Toastify.TostifyCustomClose("", "");
            }
        }
        else
        {
            var result = await ProductService.CreateAsync(ProductDto);

            if (result.IsSuccess)
            {
                Navigation.NavigateTo("/dashboard");
            }
            else
            {
                var message = result.Errors.FirstOrDefault()?.Message ?? "An error occurred.";

                //await JSRuntime.InvokeVoidAsync("notify.showToast", "error", message);
                await Toastify.TostifyCustomClose("", "");
            }
        }
    }
}
