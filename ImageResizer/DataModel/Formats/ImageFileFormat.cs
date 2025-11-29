namespace ImageResizer.DataModel.Formats;

public enum ImageFileFormat
{
    AVIF,
    WebP,
    JPEG
};

public static class ImageFileFormatsExtensions
{
    public static string ToFileExtension(this ImageFileFormat fileFormat)
    {
        switch (fileFormat)
        {
            case ImageFileFormat.AVIF:
                return "avif";
            case ImageFileFormat.WebP:
                return "webp";
            case ImageFileFormat.JPEG:
                return "jpeg";
            default:
                throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
        }
    }

    public static string ToMimeType(this ImageFileFormat fileFormat)
    {
        switch (fileFormat)
        {
            case ImageFileFormat.AVIF:
                return "image/avif";
            case ImageFileFormat.WebP:
                return "image/webp";
            case ImageFileFormat.JPEG:
                return "image/jpeg";
            default:
                throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
        }
    }
}