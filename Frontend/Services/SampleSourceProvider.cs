using Aspirgregator.Abstractions;

namespace Aspiregregator.Frontend.Services;

public sealed class SampleSourceProvider(IGrainFactory grainFactory, AppState appState) : ISourceProvider
{
    public async Task<SourceItem?> GetSourceItemAsync(string endpoint)
    {
        if (grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty)
                        .GetSourceAsync(endpoint) is not null)
        {
            return await grainFactory.GetGrain<ISourceGrain>(endpoint)
                                     .GetSourceAsync();
        }

        return null;
    }

    public async Task<IEnumerable<SourceItem>> GetSourcesAsync()
      => (await grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty)
                            .GetSourcesAsync())
                                .OrderBy(x => x.Name)
                                .AsEnumerable();


    public async Task SaveSourceItemAsync(SourceItem item)
    {
        await grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty)
                          .CreateSource(item.Endpoint);

        appState.AppStateChanged();
    }

    public async Task<SourceItem> UpdateAsync(SourceItem source)
    {
        source = await grainFactory.GetGrain<ISourceGrain>(source.Endpoint)
                                   .UpdateSourceAsync(source);

        appState.AppStateChanged();

        return source;
    }

    public async Task RemoveSourceAsync(SourceItem item)
    {
        await grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty).RemoveSourceAsync(item);

        appState.AppStateChanged();
    }
}