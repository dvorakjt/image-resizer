using System.Collections;
using System.Collections.Specialized;

namespace ImageResizer.ViewModels;

public class LiveSortedList<T>: IEnumerable<T>, INotifyCollectionChanged where T: IComparable<T>
{
    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}