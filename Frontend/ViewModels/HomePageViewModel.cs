namespace Aspiregregator.Frontend.ViewModels;

public class HomePageViewModel(ISourceProvider sourceProvider)
{
    public IQueryable<EntryItem>? EntryItems { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;

    public async Task RefreshAsync()
    {
        var sources = await sourceProvider.GetSourcesAsync();

        List<EntryItem> entries = [];

        foreach (var source in sources)
            foreach (var entry in source.MostRecentItems) entries.Add(entry);

        EntryItems = entries.OrderByDescending(x => x.UpdatedDate)
            .AsQueryable()
            .Skip(PageIndex * PageSize)
            .Take(PageSize);
    }
}