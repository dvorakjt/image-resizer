using System.Collections.ObjectModel;

namespace ImageResizer.Models;

public class WidthsModeFormData : AbstractFormData
{
    public WidthComparisonMode WidthComparisonMode { get; }
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
}