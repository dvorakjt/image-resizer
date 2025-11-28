using ImageResizer.DataModel.Formats;
using NetVips;

namespace ImageResizer.ImageProcessing;

public class NetVipsImageWriter : IImageWriter
{
    public async Task ResizeReformatAndSaveAsAVIF(byte[] imageData, HashSet<int> widths, IImagePath outputPath, int quality, int effort)
    {
        CreateDirectoryIfNotExists(outputPath, ImageFileFormats.AVIF);
        var image = NetVips.Image.NewFromBuffer(imageData);
        await Task.WhenAll(widths.Select(w => ResizeReformatAndSaveOneAsAVIF(image, w, outputPath, quality, effort)));
        image.Dispose();
    }

    public async Task ResizeReformatAndSaveAsWebP(byte[] imageData, HashSet<int> widths, IImagePath outputPath, int quality, int effort)
    {        
        CreateDirectoryIfNotExists(outputPath, ImageFileFormats.WebP);
        var image = NetVips.Image.NewFromBuffer(imageData);
        await Task.WhenAll(widths.Select(w => ResizeReformatAndSaveOneAsWebP(image, w, outputPath, quality, effort)));
        image.Dispose();
    }

    public async Task ResizeReformatAndSaveAsJPEG(byte[] imageData, HashSet<int> widths, IImagePath outputPath, int quality)
    {
        CreateDirectoryIfNotExists(outputPath, ImageFileFormats.JPEG);
        var image = NetVips.Image.NewFromBuffer(imageData);
        await Task.WhenAll(widths.Select(w => ResizeReformatAndSaveOneAsJPEG(image, w, outputPath, quality)));
        image.Dispose();
    }

    private Task ResizeReformatAndSaveOneAsAVIF(
        NetVips.Image image,
        int width,
        IImagePath outputPath,
        int quality,
        int effort
    )
    {
        return Task.Run(() =>
        {
            var filepath = outputPath.GetPlatformSpecificFilePath(ImageFileFormats.AVIF, width);
            using var resizedImage = ResizeImage(image, width);
            resizedImage.Heifsave(filepath, quality, effort: effort);
        });
    }
    
    private Task ResizeReformatAndSaveOneAsWebP(
        NetVips.Image image,
        int width,
        IImagePath outputPath,
        int quality,
        int effort
    )
    {
        return Task.Run(() =>
        {
            var filepath = outputPath.GetPlatformSpecificFilePath(ImageFileFormats.WebP, width);
            using var resizedImage = ResizeImage(image, width);
            resizedImage.Webpsave(filepath, quality, effort: effort);
        });
    }
    
    private Task ResizeReformatAndSaveOneAsJPEG(
        NetVips.Image image,
        int width,
        IImagePath outputPath,
        int quality
    )
    {
        return Task.Run(() =>
        {
            var filepath = outputPath.GetPlatformSpecificFilePath(ImageFileFormats.JPEG, width);
            using var resizedImage = ResizeImage(image, width);
            resizedImage.Jpegsave(filepath, quality);
        });
    }

    private void CreateDirectoryIfNotExists(IImagePath outputPath, ImageFileFormats format)
    {
        var directoryPath = outputPath.GetPlatformSpecificDirPath(format);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
    
    private NetVips.Image ResizeImage(NetVips.Image originalImage, int newWidth)
    {
        var scale = (double)newWidth / originalImage.Width;
        return originalImage.Resize(scale);
    }
}