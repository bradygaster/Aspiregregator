using SimpleFeedReader;

namespace Aspiregregator.Frontend.Services;

public class SampleSourceProvider : ISourceProvider
{
    public static List<SourceItem> Sources = new()
    {
        new SourceItem { Endpoint = "https://devblogs.microsoft.com/dotnet/feed/", Name = ".NET Blog"},
        new SourceItem { Endpoint = "https://www.hanselman.com/blog/feed/rss", Name = "Scott Hanselman's Blog"},
        new SourceItem { Endpoint = "https://devblogs.microsoft.com/visualstudio/feed/", Name = "Visual Studio Blog" },
        new SourceItem { Endpoint = "https://github.com/dotnet/aspire/commits.atom", Name = ".NET Aspire (Commits)" }
    };

    public Task<SourceItem?> GetSourceItemAsync(string endpoint)
        => Task.FromResult(Sources.FirstOrDefault(x => x.Endpoint == endpoint));

    public Task<IEnumerable<SourceItem>> GetSourcesAsync()
        => Task.FromResult(Sources.AsEnumerable());

    public Task SaveSourceItemAsync(SourceItem item)
    {
        if (!Sources.Any(x => x.Endpoint == item.Endpoint))
            Sources.Add(item);

        return Task.CompletedTask;
    }

    public Task<SourceItem> UpdateAsync(SourceItem source)
    {
        source.MostRecentItems = new FeedReader().RetrieveFeed(source.Endpoint).Select(x => new EntryItem
        {
            Title = x.Title,
            Description = x.Summary,
            Link = x.Uri.AbsoluteUri,
            PublishDate = x.PublishDate,
            UpdatedDate = x.LastUpdatedDate
        }).ToList();

        return Task.FromResult(source);
    }
}
