namespace ImageResizer.DataModel;

public record class FormElementState<T> : IFormElementState<T>
{
    public required T Value { get; init; }
    public bool IsValid { get; init; }
    public required string ErrorMessage { get; init;  }
}