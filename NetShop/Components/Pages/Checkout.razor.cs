using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using NetShop.Data;
using NetShop.Interfaces;

namespace NetShop.Components.Pages;

public partial class Checkout : ComponentBase
{
    [Parameter] public required string StripePriceId { get; set; }

    [Inject] private IStripeService StripeService { get; set; } = default!;

    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var users = UserManager.Users;

        var demoUser = users.FirstOrDefault();

        var result = await StripeService.CreateSessionAsync(demoUser, StripePriceId);

        if (result.IsSuccess && result.Value != null)
        {
            Navigation.NavigateTo(result.Value.Url, true);
        }
    }
}
