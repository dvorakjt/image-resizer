namespace ImageResizer.DataModel;

public class ValidatorResult : IValidatorResult
{
    public bool IsValid { get; init; }
    public required string ErrorMessage { get; init; }
}