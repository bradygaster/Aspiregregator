using CodeHollow.FeedReader;

namespace Aspiregregator.Frontend.Services;

public class SampleSourceProvider : ISourceProvider
{
    public static List<SourceItem> Sources = new()
    {
        new SourceItem { Endpoint = "https://devblogs.microsoft.com/dotnet/feed/", Name = ".NET Blog"},
        new SourceItem { Endpoint = "https://www.hanselman.com/blog/feed/rss", Name = "Scott Hanselman's Blog"},
        new SourceItem { Endpoint = "https://devblogs.microsoft.com/visualstudio/feed/", Name = "Visual Studio Blog" }
    };

    public async Task<IEnumerable<EntryItem>> GetEntriesAsync(SourceItem item)
    {
        var feed = await FeedReader.ReadAsync(item.Endpoint);
        return feed.Items.Select(x => new EntryItem
        { 
            Title = x.Title,
            Description = x.Description,
            Link = x.Link,
            Category = x.Categories.Any() ? x.Categories.FirstOrDefault() : null,
            PublishDate = x.PublishingDate.HasValue ? x.PublishingDate.Value : DateTimeOffset.MinValue,
        });
    }

    public Task<SourceItem?> GetSourceItemAsync(string endpoint)
        => Task.FromResult(Sources.FirstOrDefault(x => x.Endpoint == endpoint));

    public Task<IEnumerable<SourceItem>> GetSourcesAsync() 
        => Task.FromResult(Sources.AsEnumerable());

    public Task SaveSourceItemAsync(SourceItem item)
    {
        if(!Sources.Any(x => x.Endpoint == item.Endpoint))
            Sources.Add(item);

        return Task.CompletedTask;
    }
}
