using Aspiregregator;

namespace Aspirgregator.Abstractions;

[Alias("Aspirgregator.Abstractions.ISourceLibraryGrain")]
public interface ISourceLibraryGrain : IGrainWithGuidKey
{
    Task<IEnumerable<SourceItem>> GetSourcesAsync();
    Task<ISourceGrain?> CreateSource(string endpoint);
    Task<ISourceGrain?> GetSourceAsync(string endpoint);
    Task RemoveSourceAsync(SourceItem item);
}
