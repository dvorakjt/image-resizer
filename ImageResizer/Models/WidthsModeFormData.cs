using System.Collections.ObjectModel;

namespace ImageResizer.Models;

public class WidthsModeFormData : AbstractFormData
{
    public WidthComparisonMode WidthComparisonMode { get; }
    public uint DefaultImageWidth { get; }
    public IReadOnlyDictionary<uint, uint> Widths { get; }

public WidthsModeFormData(
        Stream imageStream,
        string outputFileName, 
        uint versionNumber,
        string pathToPublicDirectory,
        string pathFromPublicDirectory, 
        string altText, 
        IEnumerable<AbstractImageFormatData> imageFormats,
        WidthComparisonMode widthComparisonMode,
        uint defaultImageWidth,
        IDictionary<uint, uint> widths): base(imageStream, outputFileName, versionNumber, pathToPublicDirectory, pathFromPublicDirectory, altText, imageFormats)
    {
        this.WidthComparisonMode = widthComparisonMode;
        this.DefaultImageWidth = defaultImageWidth;
        this.Widths = new ReadOnlyDictionary<uint, uint>(widths);
    }
}