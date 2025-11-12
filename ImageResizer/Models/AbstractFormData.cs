using System.Text;

namespace ImageResizer.Models;

public abstract class AbstractFormData
{
    public Stream ImageStream { get; }
    public string OutputFileName { get; }
    public uint VersionNumber { get; }
    public string PathToPublicDirectory { get; }
    public string PathFromPublicDirectory { get; }
    public string AltText { get; }
    
    public IList<AbstractImageFormatData> ImageFormats { get; }

    protected AbstractFormData(Stream imageStream, string outputFileName, uint versionNumber, string pathToPublicDirectory,
        string pathFromPublicDirectory, string altText, IEnumerable<AbstractImageFormatData> imageFormats)
    {
        ImageStream = imageStream;
        OutputFileName = outputFileName;
        VersionNumber = versionNumber;
        PathToPublicDirectory = pathToPublicDirectory;
        PathFromPublicDirectory = pathFromPublicDirectory;
        AltText = altText;
        ImageFormats = imageFormats.ToList();
    }
}