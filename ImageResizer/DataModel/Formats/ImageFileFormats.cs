namespace ImageResizer.DataModel.Formats;

public enum ImageFileFormats
{
    AVIF,
    WebP,
    JPEG
};

public static class ImageFileFormatsExtensions
{
    public static string ToFileExtension(this ImageFileFormats fileFormat)
    {
        switch (fileFormat)
        {
            case ImageFileFormats.AVIF:
                return ".avif";
            case ImageFileFormats.WebP:
                return ".webp";
            case ImageFileFormats.JPEG:
                return ".jpeg";
            default:
                throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
        }
    }
}