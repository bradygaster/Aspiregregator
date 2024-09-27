namespace Aspiregregator.Frontend.ViewModels;

public class EntryItemViewModel(EntryItem baseItem, SourceItem source)
{
    public string Title => baseItem.Title;
    public string Link => baseItem.Link;
    public string Source => source.Name;
    public DateTimeOffset DisplayDate => baseItem.GetDisplayDate();
}
