namespace ImageResizer.Components;

public class FormElementStateChangedEventArgs<T> : EventArgs
{
    public FormElementState<T> State { get; }

    public FormElementStateChangedEventArgs(FormElementState<T> state)
    {
        State = state;
    }
}