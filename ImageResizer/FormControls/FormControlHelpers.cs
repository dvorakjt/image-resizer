using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

public static class FormControlHelpers
{
    public static Func<string, ValidatorResult> CreateRequiredFieldValidator(string errorMessage)
    {
        return (string value) =>
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            return new ValidatorResult
            {
                IsValid = isValid,
                ErrorMessage = isValid ? "" : errorMessage
            };
        };
    }

    public static Func<string, ValidatorResult> CreateMinMaxValidator(int minValue, int maxValue,
        string errorMessage)
    {
        return (string value) =>
        {
            var canParse = int.TryParse(value, out var width);

            if (canParse)
            {
                if (width >= minValue && width <= maxValue)
                {
                    return new ValidatorResult
                    {
                        IsValid = true,
                        ErrorMessage = ""
                    };
                }
            }

            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        };
    }

    public static Func<T, ValidatorResult> ChainValidators<T>(IEnumerable<Func<T, ValidatorResult>> validators)
    {
        return (T value) =>
        {
            foreach (var validator in validators)
            {
                var result = validator(value);
                if (result.IsValid) return result;
            }

            return new ValidatorResult
            {
                IsValid = true,
                ErrorMessage = ""
            };
        };
    }
    
    public static string ToIntegerOrEmptyString(string value, bool allowZero)
    {
        if (allowZero && value.StartsWith("0")) return "0";

        var result = "";
        bool foundNonZeroValue = false;
        foreach (var c in value)
        {
            if (c == '0')
            {
                if (foundNonZeroValue) result += c;
            } else if (IsDigit(c))
            {
                result += c;
                foundNonZeroValue = true;
            }
        }

        return result;
    }

    public static bool IsIntegerOrEmptyString(string value, bool allowZero)
    {
        if (allowZero && value == "0") return true;
        
        var foundNonZeroInteger = false;
        foreach (var c in value)
        {
            if (!IsDigit(c)) return false;
            if(c == '0' && !foundNonZeroInteger) return false;
            foundNonZeroInteger = true;
        }

        return true;
    }

    private static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }
}