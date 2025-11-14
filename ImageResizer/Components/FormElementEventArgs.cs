namespace ImageResizer.Components;

public class FormElementEventArgs<T> : EventArgs
{
    public T Value { get; }
    public Validity Validity { get; }

    public FormElementEventArgs(T value, Validity validity)
    {
        Value = value;
        Validity = validity;
    }
}