namespace ImageResizer.DataModel;

/// <summary>
/// An entity that stores a value, emits state updates, and can be validated, revalidated, and reset.
/// </summary>
/// <typeparam name="T">
/// The type of value stored within the element's State.Value property.
/// </typeparam>
public interface IFormElement<out T> : IStateful<T>, IRevalidatable, IResettable
{
}