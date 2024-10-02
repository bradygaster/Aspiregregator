namespace Aspiregregator.Frontend.ViewModels;

public class HomePageViewModel(ISourceProvider sourceProvider)
{
    public IQueryable<EntryItemViewModel>? EntryItems { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public async Task RefreshAsync()
    {
        var sources = await sourceProvider.GetSourcesAsync();

        List<EntryItemViewModel> entries = [];
        foreach (var source in sources)
        {
            foreach (var entry in source.MostRecentItems)
            {
                entries.Add(new EntryItemViewModel(entry, source));
            }
        }

        EntryItems = entries.OrderByDescending(x => x.DisplayDate)
            .AsQueryable()
            .Skip(PageIndex * PageSize)
            .Take(PageSize);
    }
}