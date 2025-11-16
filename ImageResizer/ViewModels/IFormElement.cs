namespace ImageResizer.ViewModels;

public interface IFormElement<T>
{
    FormElementState<T> State { get; }
    event EventHandler<FormElementStateChangedEventArgs<T>> StateChanged;
}