﻿namespace Aspiregregator;

public class SourceItem
{
    public required string Endpoint { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
    public List<EntryItem> MostRecentItems { get; set; } = new();
}