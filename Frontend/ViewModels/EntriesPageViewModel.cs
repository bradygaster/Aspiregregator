namespace Aspiregregator.Frontend.ViewModels;

public class EntriesPageViewModel(ISourceProvider sourceProvider)
{
    public SourceItem? SelectedSource { get; set; }

    internal async Task SelectSource(string sourceSlug)
    {
        var sources = await sourceProvider.GetSourcesAsync();
        SelectedSource = sources.FirstOrDefault(x => x.GetSlug() == sourceSlug);
    }
}
