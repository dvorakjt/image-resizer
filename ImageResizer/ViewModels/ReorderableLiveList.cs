using System.Collections;

namespace ImageResizer.ViewModels;

public class ReorderableLiveList<T> : ILiveList<T>
{
    public event EventHandler<ListItemAddedEventArgs<T>>? ItemAdded;
    public event EventHandler<ListItemMovedEventArgs>? ItemMoved;
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

    public void Move(T item, int toIndex)
    {
        if (toIndex < 0 || toIndex >= _items.Count) return;
        
        var oldIndex = _items.IndexOf(item);
        if (oldIndex == -1 || oldIndex == toIndex) return;
        
        _items.RemoveAt(oldIndex);
        _items.Insert(toIndex, item);
        
        ItemMoved?.Invoke(this, new ListItemMovedEventArgs()
        {
            OldIndex = oldIndex,
            NewIndex = toIndex,
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