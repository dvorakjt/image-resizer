namespace ImageResizer.DataModel;

public interface IFormElementState<out T> : IValidatorResult
{
    T Value { get; }
}