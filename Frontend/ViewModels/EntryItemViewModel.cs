namespace Aspiregregator.Frontend.ViewModels;

public class EntryItemViewModel(EntryItem baseItem, SourceItem source)
{
    public string Title => baseItem.Title;
    public string Link => baseItem.Link;
    public string Source => source.Name;
    public DateTimeOffset DisplayDate => baseItem.GetDisplayDate();
}

public class HomePageViewModel(ISourceProvider sourceProvider)
{
    public async Task<IQueryable<EntryItemViewModel>> RefreshAsync()
    {
        var sources = await sourceProvider.GetSourcesAsync();
        List<EntryItemViewModel> entries = new List<EntryItemViewModel>();
        foreach (var source in sources)
        {
            var sourceEntries = await sourceProvider.UpdateAsync(source);
            foreach (var entry in sourceEntries.MostRecentItems)
            {
                entries.Add(new EntryItemViewModel(entry, source));
            }
        }
        return entries.OrderByDescending(x => x.DisplayDate).AsQueryable();
    }
}
