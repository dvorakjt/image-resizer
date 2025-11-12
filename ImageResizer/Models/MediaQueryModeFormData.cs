namespace ImageResizer.Models;

public class MediaQueryModeFormData : AbstractFormData
{
    public int DefaultImageWidth { get; }
    public IReadOnlyList<MediaQueryWithImageWidth> MediaQueriesWithImageWidths { get; }

public MediaQueryModeFormData(
        byte[] imageBuffer,
        string outputFileName, 
        int versionNumber,
        string pathToPublicDirectory,
        string pathFromPublicDirectory, 
        string altText,
        IEnumerable<AbstractImageFormatData> imageFormats,
        int defaultImageWidth,
        IEnumerable<MediaQueryWithImageWidth> mediaQueriesWithImageWidths): base(imageBuffer, outputFileName, versionNumber, pathToPublicDirectory, pathFromPublicDirectory, altText, imageFormats)
    {
        DefaultImageWidth = defaultImageWidth;
        MediaQueriesWithImageWidths = new List<MediaQueryWithImageWidth>(mediaQueriesWithImageWidths);
    }

    protected override IEnumerable<int> GetImageWidths()
    {
        return MediaQueriesWithImageWidths.Select(mqWithImgWidth => mqWithImgWidth.ImageWidth).Append(DefaultImageWidth).Distinct();
    }
}