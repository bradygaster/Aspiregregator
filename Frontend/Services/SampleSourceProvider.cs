using SimpleFeedReader;

namespace Aspiregregator.Frontend.Services;

public class SampleSourceProvider : ISourceProvider
{
    private static List<SourceItem> Sources = new()
    {
        //new SourceItem { Endpoint = "https://www.hanselman.com/blog/feed/rss" },
        //new SourceItem { Endpoint = "https://devblogs.microsoft.com/dotnet/feed/" },
        //new SourceItem { Endpoint = "https://devblogs.microsoft.com/visualstudio/feed/"  },
        //new SourceItem { Endpoint = "https://github.com/dotnet/aspire/commits.atom"  },
        //new SourceItem { Endpoint = "https://github.com/dotnet/aspire/releases.atom"  }
    };

    public Task<SourceItem?> GetSourceItemAsync(string endpoint)
        => Task.FromResult(Sources.FirstOrDefault(x => x.Endpoint == endpoint));

    public Task<IEnumerable<SourceItem>> GetSourcesAsync()
        => Task.FromResult(Sources.OrderBy(x => x.Name).AsEnumerable());

    public Task SaveSourceItemAsync(SourceItem item)
    {
        if (!Sources.Any(x => x.Endpoint == item.Endpoint))
        {
            Sources.Add(item);
        }

        return Task.CompletedTask;
    }

    public async Task<SourceItem> UpdateAsync(SourceItem source)
    {
        source.MostRecentItems = new FeedReader().RetrieveFeed(source.Endpoint).Select(x => new EntryItem
        {
            Title = x.Title,
            Description = x.Summary,
            Link = x.Uri.AbsoluteUri,
            PublishDate = x.PublishDate,
            UpdatedDate = x.LastUpdatedDate
        }).ToList();

        source.Name = (await CodeHollow.FeedReader.FeedReader.ReadAsync(source.Endpoint)).Title;
        Sources.First(x => x.Endpoint == source.Endpoint).MostRecentItems = source.MostRecentItems;
        Sources.First(x => x.Endpoint == source.Endpoint).Name = source.Name;
        Sources.First(x => x.Endpoint == source.Endpoint).LastUpdate = source.LastUpdate;

        return source;
    }
}
