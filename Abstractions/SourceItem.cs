namespace Aspiregregator;

[GenerateSerializer]
public class SourceItem
{
    [Id(0)]
    public required string Endpoint { get; set; }
    [Id(1)]
    public required string Name { get; set; }
    [Id(2)]
    public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
    [Id(3)]
    public List<EntryItem> MostRecentItems { get; set; } = [];
}
