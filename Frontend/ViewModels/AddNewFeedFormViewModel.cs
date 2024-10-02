using Aspiregregator.Frontend.Services;

namespace Aspiregregator.Frontend.ViewModels;

public class AddNewFeedFormViewModel(ISourceProvider sourceProvider,
    AppState appState)
{
    public string FeedUri { get; set; } = string.Empty;

    public async Task HandleSubmit()
    {
        if (IsValidUrl())
        {
            var newSource = new SourceItem { Endpoint = FeedUri, Name = "Untitled" };
            await sourceProvider.SaveSourceItemAsync(newSource);
            await sourceProvider.UpdateAsync(newSource);

            FeedUri = string.Empty;

            appState.AppStateChanged();
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

