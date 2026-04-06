using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NetShop.Interfaces;
using NetShop.Models;

namespace NetShop.Components.Pages;

public partial class Login : ComponentBase
{
    [Inject] private IAuthService AuthService { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject] private ILocalStorageService LocalStorageService { get; set; } = default!;

    [Inject] private NavigationManager Navigation { get; set; } = default!;

    protected LoginForm loginModel = new();

    protected async Task HandleLogin()
    {
        var result = await AuthService.Login(loginModel);

        if (result.IsSuccess)
        {
            await LocalStorageService.SetItemAsync("authToken", result.Value.Token);

            Navigation.NavigateTo("/dashboard");
        }
        else
        {
            var message = result.Errors?.FirstOrDefault()?.Message ?? "An unexpected error occurred.";

            await JSRuntime.InvokeVoidAsync("notify.showToast", "error", message);
        }
    }
}
