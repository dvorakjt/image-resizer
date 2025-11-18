namespace ImageResizer.DataModel;

/// <summary>
/// An entity that stores a value together its validity and associated error messages, emits state updates, can be
/// revalidated, can display errors, and can be reset.
/// </summary>
/// <typeparam name="T">
/// The type of value stored within the element's State.Value property.
/// </typeparam>
public interface IFormElement<out T>
{
    IFormElementState<T> State { get; }
    event EventHandler<IFormElementState<T>> StateChanged;
    void Revalidated();
    void DisplayErrors();
    void Reset();
}

