using Microsoft.JSInterop;

namespace Aspiregregator.Frontend.Extensions;

internal static class JSRuntimeExtensions
{
    internal static async ValueTask SetItemAsync<T>(this IJSRuntime js, string key, T item)
    {
        await js.InvokeVoidAsync(identifier: "window.localStorage.setItem", key, item);
    }

    internal static async ValueTask<T> GetItemAsync<T>(this IJSRuntime js, string key)
    {
        return await js.InvokeAsync<T>(identifier: "window.localStorage.getItem", key);
    }
}
