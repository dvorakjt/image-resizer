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
    
    protected override string CreateSourceOrImgElement(AbstractImageFormatData imageFormat)
    {
        var srcset = CreateSrcSet(imageFormat.GetExtension());
        var sizes = CreateSizes();

        if (imageFormat is AVIFImageFormatData || imageFormat is WebPImageFormatData)
        {
            var source = $"<source srcset=\"{srcset}\" sizes=\"{sizes}\" type=\"{imageFormat.GetMimeType()}\" />";
            return source;
        }
        
        return $"<img srcset=\"{srcset}\" sizes=\"{sizes}\" alt=\"{AltText}\" />";
    }

    private string CreateSrcSet(string ext)
    {
        var imageWidths = GetImageWidths().ToList();
        
        var sources = imageWidths.Select(imageWidth =>
            $"{OutputPath.ToRelativeFilePathString(imageWidth, ext)} {imageWidth}w"
        );

        return string.Join(", ", sources);
    }

    private string CreateSizes()
    {
        var sizes =
            MediaQueriesWithImageWidths.Select(mqWithImageWidth => $"({mqWithImageWidth.MediaQuery}) {mqWithImageWidth.ImageWidth}px");
        sizes = sizes.Append($"{DefaultImageWidth}px");
        return string.Join(", ", sizes);
    }
}