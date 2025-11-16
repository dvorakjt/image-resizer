using System.Collections;

namespace ImageResizer.ViewModels;

public class LiveSortedList<T> : ILiveSortedList<T> where T : IComparable<T>
{
    public event EventHandler<ListItemAddedEventArgs<T>>? ItemAdded;
    public event EventHandler<ListItemRemovedEventArgs>? ItemRemoved;
    public event EventHandler? ListReset;

    public bool IsReversed { get; set
        {
            var reversed = value;

            if (field != reversed)
            {
                field = reversed;
                _items.Reverse();
                ListReset?.Invoke(this, EventArgs.Empty);
            }
        }
}

    private List<T> _items = new List<T>();
    
    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        var newIndex = InsertInPlace(item);
        ItemAdded?.Invoke(this, new ListItemAddedEventArgs<T>{ NewItem = item, NewIndex = newIndex} );
    }
    public void Remove(T item)
    {
        var index = _items.IndexOf(item);
        _items.RemoveAt(index);
        ItemRemoved?.Invoke(this, new ListItemRemovedEventArgs { OldIndex = index });
    }

    private int InsertInPlace(T item)
    {
        int i = 0;

        for (; i < _items.Count; i++)
        {
            if (CompareItems(item, _items[i]) < 0)
            {
                break;
            }
        }
        
        _items.Insert(i, item);
        return i;
    }

    private int CompareItems(T a, T b)
    {
        return IsReversed ? b.CompareTo(a) : a.CompareTo(b);
    }
}