namespace ImageResizer.Utils;

public interface ISortedLiveList<T> : ILiveList<T> where T : IComparable<T>
{
    bool IsReversed { get; set; }

}