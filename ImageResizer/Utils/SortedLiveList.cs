using System.Collections;

namespace ImageResizer.Utils;

public class SortedLiveList<T> : ISortedLiveList<T> where T : IComparable<T>
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

    private List<T> _items = [];
    
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

    public void Clear()
    {
        _items.Clear();
        ListReset?.Invoke(this, EventArgs.Empty);
    }
    
    public int IndexOf(T item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Equals(item)) return i;
        }

        return -1;
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