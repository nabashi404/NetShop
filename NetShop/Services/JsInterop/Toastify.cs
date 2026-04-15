using Microsoft.JSInterop;

namespace NetShop.Services.JsInterop;

public class Toastify(IJSRuntime jsRuntime)
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public async Task TostifyCustomClose(string type, string message)
    {
        var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/toastify.service.js");

        await module.InvokeVoidAsync("showToast", type, message);
    }
}
