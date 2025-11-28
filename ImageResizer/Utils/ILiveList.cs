namespace ImageResizer.Utils;

public class ListItemAddedEventArgs<T> : EventArgs
{
    public required T NewItem { get; init; }
    public required int NewIndex { get; init; }
}

public class ListItemRemovedEventArgs : EventArgs
{
    public required int OldIndex { get; init; }
}

public interface ILiveList<T> : IEnumerable<T>
{
    event EventHandler<ListItemAddedEventArgs<T>> ItemAdded;
    event EventHandler<ListItemRemovedEventArgs> ItemRemoved;
    event EventHandler ListReset;
    
    void Add(T item);
    void Remove(T item);
    int IndexOf(T item);
    void Clear();
}

