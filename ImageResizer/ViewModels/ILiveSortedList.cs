using System.Collections.Specialized;

namespace ImageResizer.ViewModels;

public interface ILiveSortedList<T> : IEnumerable<T>, INotifyListItemAdded<T>, INotifyListItemRemoved, INotifyListItemReset
{
    bool IsReversed { get; set; }
    void Add(T item);
    void Remove(T item);
}