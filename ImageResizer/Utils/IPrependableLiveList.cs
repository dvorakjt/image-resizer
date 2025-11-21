namespace ImageResizer.Utils;

public interface IPrependableLiveList<T> : ILiveList<T>
{
    void Prepend(T item);
}