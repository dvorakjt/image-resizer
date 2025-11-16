using System.Collections.ObjectModel;

namespace ImageResizer.Models;

public class WidthsModeFormData : AbstractFormData
{
    public WidthComparisonMode WidthComparisonMode { get; }
    
    // Must be smaller than smallest image if WidthComparisonMode is GTE and larger than largest image otherwise.
    public int DefaultImageWidth { get; }
    public IReadOnlyDictionary<int, int> Widths { get; }

public WidthsModeFormData(
        byte[] imageBuffer,
        string outputFileName, 
        int versionNumber,
        string pathToPublicDirectory,
        string pathFromPublicDirectory, 
        string altText, 
        IEnumerable<AbstractImageFormatData> imageFormats,
        WidthComparisonMode widthComparisonMode,
        int defaultImageWidth,
        IDictionary<int, int> widths): base(imageBuffer, outputFileName, versionNumber, pathToPublicDirectory, pathFromPublicDirectory, altText, imageFormats)
    {
        this.WidthComparisonMode = widthComparisonMode;
        this.DefaultImageWidth = defaultImageWidth;
        this.Widths = new ReadOnlyDictionary<int, int>(widths);
    }

    protected override IEnumerable<int> GetImageWidths()
    {
        return Widths.Values.ToList().Append(DefaultImageWidth);
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
        imageWidths.Sort((a, b) => WidthComparisonMode == WidthComparisonMode.MaxWidths ? a.CompareTo(b) : b.CompareTo(a));
        
        var sources = imageWidths.Select(imageWidth =>
            $"{OutputPath.ToRelativeFilePathString(imageWidth, ext)} {imageWidth}w"
        );

        return string.Join(", ", sources);
    }

    private string CreateSizes()
    {
        var mediaQuery = WidthComparisonMode == WidthComparisonMode.MaxWidths ? "max-width" : "min-width";
        var sizes =
            Widths.Keys.Select(screenWidth => $"({mediaQuery}: {screenWidth}px) {Widths[screenWidth]}px").ToList();
        sizes.Add($"{DefaultImageWidth}px");
        return string.Join(", ", sizes);
    }
}