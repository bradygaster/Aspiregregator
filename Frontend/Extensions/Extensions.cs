namespace Aspiregregator.Frontend;

public static class SourceItemExtensions
{
    public static string GetSlug(this SourceItem sourceItem)
        => SlugGenerator.GenerateSlug(sourceItem.Name);
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

        // Find the last period before or at the maxLength
        int lastPeriodIndex = entry.Description.LastIndexOf('.', maxLength);

        // If there is no period, just trim to maxLength and add ellipsis
        if (lastPeriodIndex == -1)
        {
            return entry.Description.Substring(0, maxLength).Trim() + "...";
        }

        // Otherwise, return the substring up to the last period
        return entry.Description.Substring(0, lastPeriodIndex + 1).Trim();
    }
}