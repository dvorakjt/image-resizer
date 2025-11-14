namespace ImageResizer.Components;

public interface IFormElement<T>
{
    public event EventHandler<FormElementEventArgs<T>> StateChanged;
}