namespace ImageResizer.Components;

public class ValidationResult
{
    public Validity Validity { get; }
    public string? Message { get; }

    public ValidationResult(Validity validity, string message)
    {
        Validity = validity;
        Message = message;
    }
}