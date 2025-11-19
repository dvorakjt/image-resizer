namespace ImageResizer.DataModel;

public interface ILoadingEventEmitter
{
    public event EventHandler StartedLoading;
    public event EventHandler StoppedLoading;
}