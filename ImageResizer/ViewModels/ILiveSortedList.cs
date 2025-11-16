using System.Collections.Specialized;

namespace ImageResizer.ViewModels;

public interface ILiveSortedList<T> : ILiveList<T> where T : IComparable<T>
{
    bool IsReversed { get; set; }

}