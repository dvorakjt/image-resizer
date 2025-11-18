namespace ImageResizer.DataModel;

public interface IValidatorResult
{
    bool IsValid { get; }
    string ErrorMessage { get; }
}