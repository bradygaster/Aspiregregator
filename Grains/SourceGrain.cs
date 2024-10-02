using Aspirgregator.Abstractions;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using SimpleRssReader = SimpleFeedReader.FeedReader;

namespace Aspiregregator.Frontend.Grains;

public class SourceGrain(
    [PersistentState("FeedSource", storageName: "FeedSource")]
    IPersistentState<SourceItem> source) : Grain, ISourceGrain
{
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        source.State.Endpoint = this.GetPrimaryKeyString();

        return base.OnActivateAsync(cancellationToken);
    }

    public async Task<IEnumerable<EntryItem>> GetRecentEntries(int pageSize = 10)
    {
        await source.ReadStateAsync();

        return source.State
                        .MostRecentItems
                            .OrderByDescending(x => x.UpdatedDate)
                            .Take(pageSize)
                                .ToList();
    }

    public Task<SourceItem> GetSourceAsync()
        => Task.FromResult(source.State);

    public async Task<SourceItem> UpdateSourceAsync(SourceItem item)
    {
        var retrieveTask = Task.Run<List<EntryItem>>(() =>
        {
            var reader = new SimpleRssReader();
            var feedItems = reader.RetrieveFeed(item.Endpoint);

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

        var getFeedTask = FeedReader.ReadAsync(item.Endpoint);

        await Task.WhenAll(retrieveTask, getFeedTask);

        item.MostRecentItems = await retrieveTask;

        var feed = await getFeedTask;
        item.Name = feed.Title;

        source.State = feed.Type switch
        {
            FeedType.MediaRss => WithMediaRssImages(item, feed),
            FeedType.Rss_2_0 => WithRss20Images(item, feed),
            _ => item
        };

        await source.WriteStateAsync();

        return item;
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
