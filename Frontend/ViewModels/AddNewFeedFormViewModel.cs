using Microsoft.AspNetCore.Components;

namespace Aspiregregator.Frontend.ViewModels;

public class AddNewFeedFormViewModel(ISourceProvider sourceProvider)
{
    public string FeedUri { get; set; } = string.Empty;

    public EventCallback SourcesUpdated { get; set; }

    public async Task HandleSubmit()
    {
        if (IsValidUrl())
        {
            var newSource = new SourceItem { Endpoint = FeedUri };
            await sourceProvider.SaveSourceItemAsync(newSource);
            await sourceProvider.UpdateAsync(newSource);

            if (SourcesUpdated.HasDelegate)
            {
                await SourcesUpdated.InvokeAsync();
            }

            FeedUri = string.Empty;
        }
    }

    public bool IsValidUrl()
    {
        if (string.IsNullOrWhiteSpace(FeedUri))
        {
            return false;
        }

        if (Uri.TryCreate(FeedUri, UriKind.Absolute, out Uri? uriResult) &&
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
        {
            return true;
        }

        return false;
    }
}

