using SimpleFeedReader;
using System.Collections.Concurrent;

namespace Aspiregregator.Frontend.Services;

public sealed class SampleSourceProvider(AppState appState) : ISourceProvider
{
    private readonly ConcurrentDictionary<string, SourceItem> Sources =
      new(StringComparer.OrdinalIgnoreCase)
      {
          //new SourceItem { Endpoint = "https://devblogs.microsoft.com/dotnet/feed/" },
          //new SourceItem { Endpoint = "https://devblogs.microsoft.com/visualstudio/feed/"  },
          //new SourceItem { Endpoint = "https://www.hanselman.com/blog/feed/rss" },
          //new SourceItem { Endpoint = "https://github.com/dotnet/aspire/releases.atom"  }
          //new SourceItem { Endpoint = "https://github.com/dotnet/aspire/commits.atom"  },
      };

    public Task<SourceItem?> GetSourceItemAsync(string endpoint)
      => Task.FromResult(Sources.TryGetValue(endpoint, out var item) ? item : null);

    public Task<IEnumerable<SourceItem>> GetSourcesAsync()
      => Task.FromResult(Sources.Values.OrderBy(x => x.Name).AsEnumerable());

    public Task SaveSourceItemAsync(SourceItem item)
    {
        Sources[item.Endpoint] = item;

        return Task.CompletedTask;
    }

    public async Task<SourceItem> UpdateAsync(SourceItem source)
    {
        var retrieveTask = Task.Run<List<EntryItem>>(() =>
        {
            var feedItems = new FeedReader().RetrieveFeed(source.Endpoint);

            return
            [
              ..feedItems.Select(x => new EntryItem
                {
                  Title = x.Title,
                  Description = x.Summary,
                  Link = x.Uri.AbsoluteUri,
                  PublishDate = x.PublishDate,
                  UpdatedDate = x.LastUpdatedDate,
                  Images = x.Images
                })
            ];
        });

        var getFeedTask = CodeHollow.FeedReader.FeedReader.ReadAsync(source.Endpoint);

        await Task.WhenAll(retrieveTask, getFeedTask);

        source.MostRecentItems = retrieveTask.Result;

        var feed = getFeedTask.Result;
        source.Name = feed.Title;

        foreach (var item in source.MostRecentItems)
        {
            if (item.Images.Any() is false)
            {
                var rssItem = feed.Items.FirstOrDefault(i => i.Link == item.Link);
                //if (rssItem is { SpecificItem.Element.})
                //{
                //
                //}
            }
        }

        Sources[source.Endpoint] = source;

        appState.AppStateChanged();

        return source;
    }
}