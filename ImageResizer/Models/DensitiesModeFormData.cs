namespace ImageResizer.Models;

public class DensitiesModeFormData : AbstractFormData
{
    public uint BaseImageWidth { get; }
    public IReadOnlySet<Density> Densities { get; }

    public DensitiesModeFormData(
        Stream imageStream,
        string outputFileName, 
        uint versionNumber,
        string pathToPublicDirectory,
        string pathFromPublicDirectory, 
        string altText,
        IEnumerable<AbstractImageFormatData> imageFormats,
        uint baseImageWidth,
        IEnumerable<Density> densities): base(imageStream, outputFileName, versionNumber, pathToPublicDirectory, pathFromPublicDirectory, altText, imageFormats)
    {
        BaseImageWidth = baseImageWidth;
        Densities = new HashSet<Density>(densities);
    }
}