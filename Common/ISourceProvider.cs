namespace Aspiregregator;

public interface ISourceProvider
{
    Task<IEnumerable<SourceItem>> GetSourcesAsync();
    Task<SourceItem?> GetSourceItemAsync(string endpoint);
    Task SaveSourceItemAsync(SourceItem item);
    Task<IEnumerable<EntryItem>> GetEntriesAsync(SourceItem item);
}
