namespace ImageResizer.Components;

public struct FormElementState<T>
{
    public T Value;
    public bool IsValid;
    public string? ErrorMessage;
    
    public FormElementState(T value, bool isValid, string? errorMessage = null)
    {
        Value = value;
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
}