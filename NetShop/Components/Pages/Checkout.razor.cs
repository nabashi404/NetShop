using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using NetShop.Data;
using NetShop.Interfaces;
using NetShop.Models;

namespace NetShop.Components.Pages;

public partial class Checkout : ComponentBase
{
    [Parameter] public required long ProductId { get; set; }

    [Inject] private IStripeService StripeService { get; set; } = default!;

    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var demoUser = await UserManager.FindByEmailAsync("demo@demo.com");

        if (demoUser == null) return;

        var cart = new List<CartItem>
        {
            new() { ProductId = 1, Quantity = 2 },
            new() { ProductId = 2, Quantity = 1 },
        };

        var result = await StripeService.CreateSessionAsync(demoUser, cart, Navigation.BaseUri);

        if (result.IsSuccess && result.Value != null)
        {
            Navigation.NavigateTo(result.Value.Url, true);
        }
    }
}
