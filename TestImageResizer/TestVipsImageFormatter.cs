using NetVips;
using ImageResizer.Models;

namespace TestImageResizer;

public class TestVipsImageFormatter
{
    [Fact]
    public async Task TestSavesAVIFImage()
    {
        var pathToPublicDir = Environment.CurrentDirectory;
        var pathFromPublicDir = "temp";
        var filename = Guid.NewGuid().ToString();
        
        try
        {
            using var originalImage = NetVips.Image.Black(200, 200);
            var originalImgBuffer = originalImage.WriteToBuffer(".jpg");
            var outputPath = new OutputPath(
                pathToPublicDir,
                pathFromPublicDir,
                filename,
                1);

            var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
            var avifFormatData = new AVIFImageFormatData(100, 1);
            await formatter.ResizeReformatAndSave(avifFormatData, originalImage.Width);
            var filePath = outputPath.ToAbsoluteFilePathString(originalImage.Width, avifFormatData.GetExtension());
            Assert.True(File.Exists(filePath));
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
        finally
        {
            Directory.Delete(Path.Join(pathToPublicDir, pathFromPublicDir, filename), true);
        }
    }
    
    [Fact]
    public async Task TestSavesWebPImage()
    {
        var pathToPublicDir = Environment.CurrentDirectory;
        var pathFromPublicDir = "temp";
        var filename = Guid.NewGuid().ToString();

        try
        {
            using var originalImage = NetVips.Image.Black(200, 200);
            var originalImgBuffer = originalImage.WriteToBuffer(".jpg");
            var outputPath = new OutputPath(
                pathToPublicDir,
                pathFromPublicDir,
                filename,
                1);
        
            var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
            var webPFormatData = new WebPImageFormatData(100, 1);
            await formatter.ResizeReformatAndSave(webPFormatData, originalImage.Width);
            var filePath = outputPath.ToAbsoluteFilePathString(originalImage.Width, webPFormatData.GetExtension());
            Assert.True(File.Exists(filePath));
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
        finally
        {
            Directory.Delete(Path.Join(pathToPublicDir, pathFromPublicDir, filename), true);
        }
    }
    
    [Fact]
    public async Task TestSavesJPGImage()
    {
        var pathToPublicDir = Environment.CurrentDirectory;
        var pathFromPublicDir = "temp";
        var filename = Guid.NewGuid().ToString();

        try
        {
            using var originalImage = NetVips.Image.Black(200, 200);
            var originalImgBuffer = originalImage.WriteToBuffer(".jpg");
            var outputPath = new OutputPath(
                pathToPublicDir,
                pathFromPublicDir,
                filename,
                1);
        
            var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
            var jpgFormatData = new JPGImageFormatData(100);
            await formatter.ResizeReformatAndSave(jpgFormatData, originalImage.Width);
            var filePath = outputPath.ToAbsoluteFilePathString(originalImage.Width, jpgFormatData.GetExtension());
            Assert.True(File.Exists(filePath));
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
        finally
        {
            Directory.Delete(Path.Join(pathToPublicDir, pathFromPublicDir, filename), true);
        }
    }

    [Fact]
    public async Task TestResizesImage()
    {
        var pathToPublicDir = Environment.CurrentDirectory;
        var pathFromPublicDir = "temp";
        using var originalImage = NetVips.Image.Black(50, 50);
        var originalImgBuffer = originalImage.WriteToBuffer(".jpg"); 
        var jpgFormatData = new JPGImageFormatData(100);
        
        for (var newWidth = originalImage.Width; newWidth >= 1; newWidth--)
        {
            var filename = Guid.NewGuid().ToString();
            var outputPath = new OutputPath(
                pathToPublicDir,
                pathFromPublicDir,
                filename,
                1);

            try
            {
                var formatter = new NetVipsImageFormatter(originalImgBuffer, outputPath);
                await formatter.ResizeReformatAndSave(jpgFormatData, newWidth);
                var resizedImage =
                    NetVips.Image.NewFromFile(
                        outputPath.ToAbsoluteFilePathString(newWidth, jpgFormatData.GetExtension()));
                Assert.Equal(newWidth, resizedImage.Width);
                resizedImage.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                Directory.Delete(Path.Join(pathToPublicDir, pathFromPublicDir, filename), true);
            }
        }
    }
}