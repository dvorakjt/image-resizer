using NetVips;

namespace ImageResizer.Models;

public class NetVipsImageFormatter : AbstractImageFormatter
{
    protected override Task ResizeAndReformat(byte[] imageData, AbstractImageFormatData imageFormatData, int newWidth, string outputPath)
    {
        return Task.Run(() =>
        {
            using var originalImage = NetVips.Image.NewFromBuffer(imageData);
            using var resizedImage = ResizeImage(originalImage, newWidth);
            
            switch (imageFormatData)
            {
                case AVIFImageFormatData avifImageFormatData:
                    ReformatAndSaveAVIFImage(resizedImage, avifImageFormatData, outputPath);
                    break;
                case WebPImageFormatData webpImageFormatData:
                    ReformatAndSaveWebPImage(resizedImage, webpImageFormatData, outputPath);
                    break;
                default:
                    ReformatAndSaveJPGImage(resizedImage, imageFormatData, outputPath);
                    break;
            }
        });
    }

    private NetVips.Image ResizeImage(NetVips.Image originalImage, int newWidth)
    {
        var scale = (double)newWidth / originalImage.Width;
        return originalImage.Resize(scale);
    }

    private void ReformatAndSaveAVIFImage(NetVips.Image resizedImage, AVIFImageFormatData imageFormatData,
        string outputPath)
    {
       resizedImage.Heifsave(filename: outputPath, q: imageFormatData.Quality, effort: imageFormatData.Effort );
    }

    private void ReformatAndSaveWebPImage(NetVips.Image resizedImage, WebPImageFormatData imageFormatData, 
        string outputPath)
    {
        
        resizedImage.Webpsave(filename: outputPath, q: imageFormatData.Quality, effort: imageFormatData.Effort );
    }

    private void ReformatAndSaveJPGImage(NetVips.Image resizedImage, AbstractImageFormatData imageFormatData,
        string outputPath)
    {
        resizedImage.Jpegsave(filename: outputPath, q: imageFormatData.Quality );
    }
}