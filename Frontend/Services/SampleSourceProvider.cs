using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using SimpleRssReader = SimpleFeedReader.FeedReader;
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
          //new SourceItem { Endpoint = "https://davidpine.net/index.xml"  },
          //new SourceItem { Endpoint = "https://www.youtube.com/feeds/videos.xml?playlist_id=PLLtasoBlKS8-iLIxN5ITij2Cuxt75uwKs"  },

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
            var reader = new SimpleRssReader();
            var feedItems = reader.RetrieveFeed(source.Endpoint);

            return
            [
              ..feedItems.Select(x => new EntryItem
                {
                  Title = x.Title,
                  Description = x.Summary,
                  Link = x.Uri.AbsoluteUri,
                  PublishDate = x.PublishDate,
                  UpdatedDate = x.LastUpdatedDate,
                  Image = x.Images?.FirstOrDefault()
                })
            ];
        });

        var getFeedTask = FeedReader.ReadAsync(source.Endpoint);

        await Task.WhenAll(retrieveTask, getFeedTask);

        source.MostRecentItems = await retrieveTask;

        var feed = await getFeedTask;
        source.Name = feed.Title;

        Sources[source.Endpoint] = feed.Type switch
        {
            FeedType.MediaRss => WithMediaRssImages(source, feed),
            FeedType.Rss_2_0 => WithRss20Images(source, feed),
            _ => source
        };

        appState.AppStateChanged();

        return source;
    }

    private static SourceItem WithRss20Images(SourceItem source, Feed feed)
    {
        foreach (var i in feed.SpecificFeed.Items.Cast<Rss20FeedItem>())
        {
            if (i.Enclosure is not { } enclosure)
            {
                continue;
            }

            var entry = source.MostRecentItems.FirstOrDefault(mri => mri.Link == i.Link);
            if (entry is null)
            {
                continue;
            }

            if (enclosure.Url is not null)
                entry.Image = new(enclosure.Url);
        }

        return source;
    }

    private static SourceItem WithMediaRssImages(SourceItem source, Feed feed)
    {
        foreach (var i in feed.SpecificFeed.Items.Cast<MediaRssFeedItem>())
        {
            if (i.Media.FirstOrDefault() is not { } media)
            {
                continue;
            }

            var entry = source.MostRecentItems.FirstOrDefault(mri => mri.Link == i.Link);
            if (entry is null)
            {
                continue;
            }

            entry.Image = new(media.Url);
        }

        return source;
    }
}