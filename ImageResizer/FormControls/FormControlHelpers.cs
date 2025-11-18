using System.Text.RegularExpressions;

namespace ImageResizer.FormControls;

public static class FormControlHelpers
{
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