namespace ImageResizer.ViewModels;

public class ValidatorFuncResult
{
    public bool IsValid { get; }
    public string? Message { get; }

    public ValidatorFuncResult(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }
}