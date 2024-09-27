namespace Aspiregregator.Frontend.ViewModels;

public class NavMenuViewModel(ISourceProvider sourceProvider)
{
    public bool Expanded { get; set; } = true;
    public IQueryable<SourceItem>? Sources { get; set; } = null;

    internal async Task RefreshSources()
    {
        Sources = (await sourceProvider.GetSourcesAsync()).AsQueryable();
    }
}
