namespace ImageResizer.ViewModels;

public class ListItemAddedEventArgs<T> : EventArgs
{
    public required T NewItem { get; init; }
    public required int NewIndex { get; init; }
}

public interface INotifyListItemAdded<T>
{
    public event EventHandler<ListItemAddedEventArgs<T>> ItemAdded;
}