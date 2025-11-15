namespace ImageResizer.Components;

public interface IFormElement<T>
{
    public FormElementState<T> State { get; }
    public event EventHandler<FormElementStateChangedEventArgs<T>> StateChanged;
}