namespace Aspiregregator.Frontend.ViewModels;

public class HomePageViewModel(ISourceProvider sourceProvider)
{
    public IQueryable<EntryItemViewModel>? EntryItems { get; set; }

    public async Task RefreshAsync()
    {
        var sources = await sourceProvider.GetSourcesAsync();

        List<EntryItemViewModel> entries = [];
        foreach (var source in sources)
        {
            var sourceEntries = await sourceProvider.UpdateAsync(source);
            foreach (var entry in sourceEntries.MostRecentItems)
            {
                entries.Add(new EntryItemViewModel(entry, source));
            }
        }

        EntryItems = entries.OrderByDescending(x => x.DisplayDate).AsQueryable();
    }
}