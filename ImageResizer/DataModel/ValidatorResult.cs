namespace ImageResizer.DataModel;

public record class ValidatorResult : IValidatorResult
{
    public bool IsValid { get; init; }
    public required string ErrorMessage { get; init; }
}