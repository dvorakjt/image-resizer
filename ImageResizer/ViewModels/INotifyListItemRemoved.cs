namespace ImageResizer.ViewModels;

public class ListItemRemovedEventArgs :  EventArgs
{
    public required int OldIndex { get; init; }
}

public interface INotifyListItemRemoved
{
    public event EventHandler<ListItemRemovedEventArgs> ItemRemoved;
}