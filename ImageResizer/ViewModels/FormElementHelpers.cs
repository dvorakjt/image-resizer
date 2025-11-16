namespace ImageResizer.ViewModels;

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

    public static Func<string, ValidatorFuncResult> CreateMinMaxValidator(int minValue, int maxValue,
        string errorMessage)
    {
        return (string value) =>
        {
            var canParse = int.TryParse(value, out var width);

            if (canParse)
            {
                if (width >= minValue && width <= maxValue)
                {
                    return new ValidatorFuncResult(
                        true,
                        ""
                    );
                }
            }

            return new ValidatorFuncResult(
                false,
                errorMessage
            );
        };
    }

    public static string AllowOnlyDigits(string value)
    {
        var result = "";
        var detectedNonZeroDigit = false;
        
        foreach (var c in value)
        {
            if (char.IsDigit(c))
            {
                if (c != '0')
                {
                    detectedNonZeroDigit = true;
                    result += c;
                } else if (detectedNonZeroDigit)
                {
                    result += c;
                }
            }
        }

        return result;
    }
}