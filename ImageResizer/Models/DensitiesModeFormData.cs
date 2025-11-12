namespace ImageResizer.Models;

public class DensitiesModeFormData : AbstractFormData
{
    public int BaseImageWidth { get; }
    public IReadOnlySet<Density> Densities { get; }

    public DensitiesModeFormData(
        byte[] imageBuffer,
        string outputFileName, 
        int versionNumber,
        string pathToPublicDirectory,
        string pathFromPublicDirectory, 
        string altText,
        IEnumerable<AbstractImageFormatData> imageFormats,
        int baseImageWidth,
        IEnumerable<Density> densities): base(imageBuffer, outputFileName, versionNumber, pathToPublicDirectory, pathFromPublicDirectory, altText, imageFormats)
    {
        BaseImageWidth = baseImageWidth;
        Densities = new HashSet<Density>(densities);
    }

    protected override IEnumerable<int> GetImageWidths()
    {
        // This needs looking into --> is this how Vips handles subpixel sizes?
        return Densities.Select(density => (int)Math.Round(BaseImageWidth * density.ToMultiplier()));
    }
}