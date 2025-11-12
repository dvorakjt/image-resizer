namespace ImageResizer.Models;

public abstract class AbstractImageFormatter
{
    protected byte[] ImageBuffer { get; }
    protected AbstractOutputPath OutputPath { get; }

    protected AbstractImageFormatter(byte[] imageBuffer, AbstractOutputPath outputPath)
    {
        ImageBuffer = imageBuffer;
        OutputPath = outputPath;
    }
    
    public async Task ResizeReformatAndSave( AbstractImageFormatData imageFormatData, int newWidth)
    {
        CreateDirectoryIfNotExists(imageFormatData);
        await ResizeAndReformat(imageFormatData, newWidth);
    }

    private void CreateDirectoryIfNotExists(AbstractImageFormatData imageFormatData)
    {
        var ext = imageFormatData.GetExtension();
        var dirPath = OutputPath.ToAbsoluteDirPathString(ext);
        Directory.CreateDirectory(dirPath);
    }

    
    protected abstract Task ResizeAndReformat(AbstractImageFormatData imageFormatData, int newWidth);
}