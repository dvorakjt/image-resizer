namespace ImageResizer.Models;

public class MediaQueryModeFormData : AbstractFormData
{
    public uint DefaultImageWidth { get; }
    public IReadOnlyList<MediaQueryWithImageWidth> MediaQueriesWithImageWidths { get; }

public MediaQueryModeFormData(
        Stream imageStream,
        string outputFileName, 
        uint versionNumber,
        string pathToPublicDirectory,
        string pathFromPublicDirectory, 
        string altText,
        IEnumerable<AbstractImageFormatData> imageFormats,
        uint defaultImageWidth,
        IEnumerable<MediaQueryWithImageWidth> mediaQueriesWithImageWidths): base(imageStream, outputFileName, versionNumber, pathToPublicDirectory, pathFromPublicDirectory, altText, imageFormats)
    {
        DefaultImageWidth = defaultImageWidth;
        MediaQueriesWithImageWidths = new List<MediaQueryWithImageWidth>(mediaQueriesWithImageWidths);
    }
}