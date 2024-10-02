using Aspiregregator;

namespace Aspirgregator.Abstractions;

[Alias("Aspirgregator.Abstractions.ISourceGrain")]
public interface ISourceGrain : IGrainWithStringKey
{
    Task<SourceItem> UpdateSourceAsync(SourceItem item);
    Task<IEnumerable<EntryItem>> GetRecentEntries(int pageSize = 10);
    Task<SourceItem> GetSourceAsync();
}
