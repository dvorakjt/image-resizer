using NetVips;
using ImageResizer.Models;

namespace TestImageResizer;

public class TestVipsImageFormatter
{
    [Fact]
    public async Task TestSavesAVIFImage()
    {
        using var originalImage = NetVips.Image.Black(200, 200);
        var avifFormatData = new AVIFImageFormatData(100, 1);
        var outputPath = $"./temp/{Guid.NewGuid()}.avif";
        var formatter = new NetVipsImageFormatter();
        await formatter.ResizeReformatAndSave(originalImage.WriteToBuffer(".jpg"), avifFormatData, originalImage.Width, outputPath);
        Assert.True(File.Exists(outputPath));
        File.Delete(outputPath);
    }
    
    [Fact]
    public async Task TestSavesWebPImage()
    {
        using var originalImage = NetVips.Image.Black(200, 200);
        var webPFormatData = new WebPImageFormatData(100, 1);
        var outputPath = $"./temp/{Guid.NewGuid()}.webp";
        var formatter = new NetVipsImageFormatter();
        await formatter.ResizeReformatAndSave(originalImage.WriteToBuffer(".jpg"), webPFormatData, originalImage.Width, outputPath);
        Assert.True(File.Exists(outputPath));
        File.Delete(outputPath);
    }
    
    [Fact]
    public async Task TestSavesJPGImage()
    {
        using var originalImage = NetVips.Image.Black(200, 200);
        var jpgFormatData = new JPGImageFormatData(100);
        var outputPath = $"./temp/{Guid.NewGuid()}.jpg";
        var formatter = new NetVipsImageFormatter();
        await formatter.ResizeReformatAndSave(originalImage.WriteToBuffer(".jpg"), jpgFormatData, originalImage.Width, outputPath);
        Assert.True(File.Exists(outputPath));
        File.Delete(outputPath);
    }

    [Fact]
    public async Task TestResizesImage()
    {
        using var originalImage = NetVips.Image.Black(50, 50);
        var jpgFormatData = new JPGImageFormatData(100);
        var formatter = new NetVipsImageFormatter();
        for (int newWidth = originalImage.Width; newWidth >= 1; newWidth--)
        {
            var outputPath = $"./temp/{Guid.NewGuid()}.jpg";
            await formatter.ResizeReformatAndSave(originalImage.WriteToBuffer(".jpg"), jpgFormatData, newWidth, outputPath);
            var resizedImage = NetVips.Image.NewFromFile(outputPath);
            Assert.Equal(newWidth, resizedImage.Width);
            resizedImage.Dispose();
            File.Delete(outputPath);
        }
    }
}