using Aspirgregator.Abstractions;
using System.Collections.Concurrent;

namespace Aspiregregator.Frontend.Services;

public sealed class SampleSourceProvider(IGrainFactory grainFactory) : ISourceProvider
{
    private readonly ConcurrentDictionary<string, SourceItem> sources =
      new(StringComparer.OrdinalIgnoreCase)
      {
          //new SourceItem { Endpoint = "https://aspireify.net/rss" },
          //new SourceItem { Endpoint = "https://devblogs.microsoft.com/dotnet/feed/" },
          //new SourceItem { Endpoint = "https://devblogs.microsoft.com/visualstudio/feed/"  },
          //new SourceItem { Endpoint = "https://www.hanselman.com/blog/feed/rss" },
          //new SourceItem { Endpoint = "https://github.com/dotnet/aspire/releases.atom"  }
          //new SourceItem { Endpoint = "https://github.com/dotnet/aspire/commits.atom"  },
          //new SourceItem { Endpoint = "https://davidpine.net/index.xml"  }
      };

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
    }

    public async Task<SourceItem> UpdateAsync(SourceItem source)
    {
        source = await grainFactory.GetGrain<ISourceGrain>(source.Endpoint)
                                   .UpdateSourceAsync(source);

        return source;
    }

    public async Task RemoveSourceAsync(SourceItem item)
    {
        await grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty).RemoveSourceAsync(item);
    }
}