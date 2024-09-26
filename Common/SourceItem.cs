namespace Aspiregregator;

public class SourceItem
{
    public required string Endpoint { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
    public string GetSlug() => SlugGenerator.GenerateSlug(Name);
}
