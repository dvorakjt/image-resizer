using NetVips;
using ImageResizer.Models;

namespace TestImageResizer;

public class TestVipsImageFormatter
{
    [Fact]
    public async Task TestSavesAVIFImage()
    {
        using var originalImage = NetVips.Image.Black(200, 200);
        var originalImgBuffer = originalImage.WriteToBuffer(".jpg");
        var outputPath = new OutputPath(
            Environment.CurrentDirectory, 
            "temp", 
            Guid.NewGuid().ToString(), 
            1);
        
        var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
        var avifFormatData = new AVIFImageFormatData(100, 1);
        await formatter.ResizeReformatAndSave(avifFormatData, originalImage.Width);
        var filePath = outputPath.ToAbsoluteFilePathString(originalImage.Width, avifFormatData.GetExtension());
        Assert.True(File.Exists(filePath));
        File.Delete(filePath);
    }
    
    [Fact]
    public async Task TestSavesWebPImage()
    {
        using var originalImage = NetVips.Image.Black(200, 200);
        var originalImgBuffer = originalImage.WriteToBuffer(".jpg");
        var outputPath = new OutputPath(
            Environment.CurrentDirectory, 
            "temp", 
            Guid.NewGuid().ToString(), 
            1);
        
        var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
        var webPFormatData = new WebPImageFormatData(100, 1);
        await formatter.ResizeReformatAndSave(webPFormatData, originalImage.Width);
        var filePath = outputPath.ToAbsoluteFilePathString(originalImage.Width, webPFormatData.GetExtension());
        Assert.True(File.Exists(filePath));
        File.Delete(filePath);
    }
    
    [Fact]
    public async Task TestSavesJPGImage()
    {
        using var originalImage = NetVips.Image.Black(200, 200);
        var originalImgBuffer = originalImage.WriteToBuffer(".jpg");
        var outputPath = new OutputPath(
            Environment.CurrentDirectory, 
            "temp", 
            Guid.NewGuid().ToString(), 
            1);
        
        var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
        var jpgFormatData = new JPGImageFormatData(100);
        await formatter.ResizeReformatAndSave(jpgFormatData, originalImage.Width);
        var filePath = outputPath.ToAbsoluteFilePathString(originalImage.Width, jpgFormatData.GetExtension());
        Assert.True(File.Exists(filePath));
        File.Delete(filePath);
    }

    [Fact]
    public async Task TestResizesImage()
    {
        using var originalImage = NetVips.Image.Black(50, 50);
        var originalImgBuffer = originalImage.WriteToBuffer(".jpg"); 
        var jpgFormatData = new JPGImageFormatData(100);
        
        for (int newWidth = originalImage.Width; newWidth >= 1; newWidth--)
        {
            var outputPath = new OutputPath(
                Environment.CurrentDirectory, 
                "temp", 
                Guid.NewGuid().ToString(), 
                1);
            
            var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
            await formatter.ResizeReformatAndSave(jpgFormatData, newWidth);
            var resizedImage = NetVips.Image.NewFromFile(outputPath.ToAbsoluteFilePathString(newWidth, jpgFormatData.GetExtension()));
            Assert.Equal(newWidth, resizedImage.Width);
            resizedImage.Dispose();
            File.Delete(outputPath.ToAbsoluteFilePathString(newWidth, jpgFormatData.GetExtension()));
        }
    }
}