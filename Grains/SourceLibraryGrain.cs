﻿using Aspirgregator.Abstractions;

namespace Aspiregregator.Frontend.Grains;

public class SourceLibraryGrain(
    [PersistentState("FeedSourceLibrary", storageName: "FeedSourceLibrary")]
    IPersistentState<List<SourceItem>> sources) : Grain, ISourceLibraryGrain
{
    public async Task<IEnumerable<SourceItem>> GetSourcesAsync()
    {
        var tmp = new List<SourceItem>();
        await sources.ReadStateAsync();
        foreach (var source in sources.State)
        {
            tmp.Add((await GrainFactory.GetGrain<ISourceGrain>(source.Endpoint).GetSourceAsync()));
        }
        return tmp.AsEnumerable();
    }

    public async Task<ISourceGrain?> CreateSource(string endpoint)
    {
        sources.State.Add(new SourceItem { Endpoint = endpoint, Name = "Untitled" });
        await sources.WriteStateAsync();
        var sourceGrain = await GetSourceAsync(endpoint);
        var source = await sourceGrain!.GetSourceAsync();
        source = await sourceGrain.UpdateSourceAsync(source);
        return sourceGrain;
    }

    public async Task<ISourceGrain?> GetSourceAsync(string endpoint)
    {
        await sources.ReadStateAsync();
        return sources.State.Any(x => x.Endpoint == endpoint)
                ? GrainFactory.GetGrain<ISourceGrain?>(endpoint)
                : null;
    }

    public async Task RemoveSourceAsync(SourceItem item)
    {
        sources.State.RemoveAll(x => x.Endpoint.Equals(item.Endpoint));
        await sources.WriteStateAsync();
    }
}
