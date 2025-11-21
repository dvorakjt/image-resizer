using System.Collections;

namespace ImageResizer.Utils;

public class PrependableLiveList<T> : IPrependableLiveList<T>
{
    public event EventHandler<ListItemAddedEventArgs<T>>? ItemAdded;
    public event EventHandler<ListItemRemovedEventArgs>? ItemRemoved;
    public event EventHandler? ListReset;
    
    private readonly IList<T> _items = new List<T>();
    
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
        _items.Add(item);
        ItemAdded?.Invoke(this, new ListItemAddedEventArgs<T>()
        {
            NewItem = item,
            NewIndex = _items.Count - 1,
        });   
    }

    public void Prepend(T item)
    {
        _items.Insert(0, item);
        ItemAdded?.Invoke(this, new ListItemAddedEventArgs<T>()
        {
            NewItem = item,
            NewIndex = 0
        });
    }

    public void Remove(T item)
    {
        var index = _items.IndexOf(item);
        if (index == -1) return;
        
        _items.RemoveAt(index);
        ItemRemoved?.Invoke(this, new ListItemRemovedEventArgs()
        {
            OldIndex = index
        });
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Equals(item)) return i;
        }

        return -1;
    }
}