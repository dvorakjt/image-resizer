using System.Collections.Specialized;

namespace ImageResizer.ViewModels;

public interface ILiveSortedList<T> : IEnumerable<T>, INotifyListItemAdded<T>, INotifyListItemRemoved, INotifyListItemReset where T : IComparable<T>
{
    bool IsReversed { get; set; }
    void Add(T item);
    void Remove(T item);
}