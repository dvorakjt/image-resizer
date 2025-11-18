namespace ImageResizer.DataModel;

public interface IStateful<out T>
{
    IState<T> State { get; }
    event EventHandler<IState<T>> StateChanged;
}