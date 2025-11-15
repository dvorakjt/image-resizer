namespace ImageResizer.Components;

public class FormElementHelpers
{
    public static Func<string, ValidatorFuncResult> CreateRequiredFieldValidator(string errorMessage)
    {
        return (string value) =>
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            return new ValidatorFuncResult(
                isValid,
                isValid ? "" : errorMessage
            );
        };
    }

    public static string AllowOnlyDigits(string value)
    {
        var chars = value.ToCharArray();
        var digits = chars.Where(c => c >= '0' && c <= '9');
        return string.Join("", digits);
    }
}