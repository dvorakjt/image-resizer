using NetVips;

namespace ImageResizer.Models;

public class NetVipsImageFormatter : AbstractImageFormatter
{
    public NetVipsImageFormatter(byte[] imageBuffer, AbstractOutputPath outputPath) : base(imageBuffer, outputPath) {}
    
    protected override Task ResizeAndReformat(AbstractImageFormatData imageFormatData, int newWidth)
    {
        return Task.Run(() =>
        {
            using var originalImage = NetVips.Image.NewFromBuffer(ImageBuffer);
            using var resizedImage = ResizeImage(originalImage, newWidth);
            var outFilePath = OutputPath.ToAbsoluteFilePathString(newWidth, imageFormatData.GetExtension());
            
            switch (imageFormatData)
            {
                case AVIFImageFormatData avifImageFormatData:
                    ReformatAndSaveAVIFImage(resizedImage, avifImageFormatData, outFilePath);
                    break;
                case WebPImageFormatData webpImageFormatData:
                    ReformatAndSaveWebPImage(resizedImage, webpImageFormatData, outFilePath);
                    break;
                default:
                    ReformatAndSaveJPGImage(resizedImage, imageFormatData, outFilePath);
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