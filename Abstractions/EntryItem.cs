namespace Aspiregregator;

[GenerateSerializer]
public class EntryItem
{
    [Id(0)]
    public required string Title { get; set; }
    [Id(1)]
    public required string Link { get; set; }
    [Id(2)]
    public required string Description { get; set; }
    [Id(3)]
    public DateTimeOffset PublishDate { get; set; }
    [Id(4)]
    public DateTimeOffset UpdatedDate { get; set; }
    [Id(5)]
    public Uri? Image { get; set; }
}
