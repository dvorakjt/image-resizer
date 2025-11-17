namespace ImageResizer.Models;

public enum OutputFormat
{
    AVIF,
    WebP,
    JPG
}

public static class OutputFormatExtensions
{
    public static string ToFileExtension(this OutputFormat outputFormat)
    {
        switch (outputFormat)
        {
            case OutputFormat.AVIF:
                return ".avif";
            case OutputFormat.WebP:
                return ".webp";
            case OutputFormat.JPG:
                return ".jpg";
            default:
                throw new ArgumentOutOfRangeException(nameof(outputFormat), outputFormat, null);
        }
    }
}