namespace ImageResizer.Models;

public abstract class AbstractImageFormatter
{
    private static void CreateDirectoryIfNotExists(string outputPath)
    {
        if(outputPath == null) throw new ArgumentNullException(nameof(outputPath));
        
        string dirPath = Path.GetDirectoryName(outputPath);
        Directory.CreateDirectory(dirPath);
    }
    
    public async Task ResizeReformatAndSave(byte[] imageData, AbstractImageFormatData imageFormatData, int newWidth, string outputPath)
    {
        CreateDirectoryIfNotExists(outputPath);
        await ResizeAndReformat(imageData, imageFormatData, newWidth, outputPath);
    }
    
    protected abstract Task ResizeAndReformat(byte[] imageData, AbstractImageFormatData imageFormatData, int newWidth,  string outputPath);
}