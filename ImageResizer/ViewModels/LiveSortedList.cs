using System.Collections;
using System.Collections.Specialized;

namespace ImageResizer.ViewModels;

public class LiveSortedList<T> : ILiveSortedList<T> where T : IComparable<T>
{
    public event EventHandler<ListItemAddedEventArgs<T>>? ItemAdded;
    public event EventHandler<ListItemRemovedEventArgs>? ItemRemoved;
    public event EventHandler? ListReset;

    private readonly IList<T> _items = new List<T>();

    private IList<T> Items
    {
        get
        {
            return (IsReversed ? _items.OrderByDescending(item => item) : _items.OrderBy(item => item)).ToList();;
        }
    }

    public bool IsReversed
    {
        get;
        set
        {
            field = value;
            ListReset?.Invoke(this, EventArgs.Empty);
        }
    } = false;

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _items.Add(item);
        ItemAdded?.Invoke(this, new ListItemAddedEventArgs<T>
        {
            NewItem = item,
            NewIndex = Items.IndexOf(item)
        });
    }

    public void Remove(T item)
    {
        int oldIndex = Items.IndexOf(item);
        _items.RemoveAt(oldIndex);
        ItemRemoved?.Invoke(this, new ListItemRemovedEventArgs()
        {
            OldIndex = oldIndex
        });
    }
}