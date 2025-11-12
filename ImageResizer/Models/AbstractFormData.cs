using System.Text;

namespace ImageResizer.Models;

public abstract class AbstractFormData
{
    protected AbstractImageFormatter ImageFormatter { get; }
    protected AbstractOutputPath OutputPath { get; }
    protected string AltText { get; }
    
    public IList<AbstractImageFormatData> ImageFormats { get; }

    protected AbstractFormData(byte[] imageBuffer, string outputFileName, int versionNumber, string pathToPublicDirectory,
        string pathFromPublicDirectory, string altText, IEnumerable<AbstractImageFormatData> imageFormats)
    {
        var op = new OutputPath(pathToPublicDirectory, pathFromPublicDirectory, outputFileName, versionNumber);
        ImageFormatter = new NetVipsImageFormatter(imageBuffer, op);
        OutputPath = op;
        AltText = altText;
        ImageFormats = imageFormats.ToList();
    }

    public async Task Save()
    {
        var imageWidths = GetImageWidths();
        foreach (var imageFormat in ImageFormats)
        {
            await Task.WhenAll(imageWidths.Select(width => ImageFormatter.ResizeReformatAndSave(imageFormat, width)));
        }
    }

    protected abstract IEnumerable<int> GetImageWidths();
}