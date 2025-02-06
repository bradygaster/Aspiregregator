namespace Aspiregregator.Frontend.ViewModels;

public class EntryItemViewModel(EntryItem baseItem)
{
    public string Title => baseItem.Title;
    public string Link => baseItem.Link;
    public string Source => baseItem.Source!.Name;
    public DateTimeOffset DisplayDate => baseItem.GetDisplayDate();
}
