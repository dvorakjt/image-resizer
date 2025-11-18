namespace ImageResizer.DataModel;

public interface IState<out T>
{
    T Value { get; }
    bool IsValid { get; }
    string ErrorMessage { get; }
}