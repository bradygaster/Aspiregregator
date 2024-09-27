using Aspiregregator.Frontend.ViewModels;

namespace Aspiregregator.Frontend;

public static class SourceItemExtensions
{
    public static string GetSlug(this SourceItem sourceItem)
        => SlugGenerator.GenerateSlug(sourceItem.Name);

    public static IQueryable<EntryItemViewModel> GetRecentEntries(this IEnumerable<SourceItem> sources)
    {
        List<EntryItemViewModel> entries = new List<EntryItemViewModel>();
        foreach (var source in sources)
        {
            foreach (var entry in source.MostRecentItems)
            {
                entries.Add(new EntryItemViewModel(entry, source));
            }
        }

        return entries.OrderByDescending(x => x.DisplayDate).AsQueryable();
    }
}

public static class EntryItemExtensions
{
    public static DateTimeOffset GetDisplayDate(this EntryItem entry)
        => entry.PublishDate > entry.UpdatedDate
            ? entry.PublishDate : entry.UpdatedDate;

    public static string GetTrimmedDescription(this EntryItem entry, int maxLength = 127)
    {
        if (string.IsNullOrEmpty(entry.Description) || entry.Description.Length <= maxLength)
        {
            return entry.Description;
        }

        int lastPeriodIndex = entry.Description.LastIndexOf('.', maxLength);

        if (lastPeriodIndex == -1)
        {
            return entry.Description.Substring(0, maxLength).Trim() + "...";
        }

        return entry.Description.Substring(0, lastPeriodIndex + 1).Trim();
    }
}