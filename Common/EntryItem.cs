namespace Aspiregregator;

public class EntryItem
{
    public required string Title { get; set; }
    public required string Link { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset PublishDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public Uri? Image { get; set; }
}
