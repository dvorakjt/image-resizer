using HtmlAgilityPack;
using ImageResizer.Models;

namespace TestImageResizer.Models;

public class TestWidthsModeFormData
{
    [Fact]
    public async Task TestWidthsModeFormDataSave()
    {
        var filename = Guid.NewGuid().ToString();
        var pathToPublicDir = Environment.CurrentDirectory;
        var pathFromPublicDir = "temp";
        var pathToOutputDir = Path.Join(pathToPublicDir, pathFromPublicDir, filename);
        
        try
        {
            using var originalImage = NetVips.Image.Black(400, 400);
            var bytes = originalImage.WriteToBuffer(".jpg");
            var versionNumber = 1;
            var altText = "A black square";

            var imageFormats = new List<AbstractImageFormatData>();
            imageFormats.Add(new AVIFImageFormatData(100, 1));
            imageFormats.Add(new WebPImageFormatData(100, 1));
            imageFormats.Add(new JPGImageFormatData(100));

            var widths = new Dictionary<int, int>();
            widths.Add(100, 100);
            widths.Add(200, 200);
            widths.Add(300, 300);
            
            var formData = new WidthsModeFormData(
                bytes,
                filename,
                versionNumber,
                pathToPublicDir,
                pathFromPublicDir,
                altText,
                imageFormats,
                WidthComparisonMode.LTE,
                originalImage.Width,
                widths);

            var pictureTag = await formData.Save();
            var pictureElement = new HtmlDocument();
            pictureElement.LoadHtml(pictureTag);
            Assert.True(pictureElement.ParseErrors.Count() == 0);

            Assert.True(Directory.Exists(pathToOutputDir));

            var extensions = new List<string> { "avif", "webp", "jpg" };
            Assert.True(extensions.All(ext => Directory.Exists(Path.Join(pathToOutputDir, ext))));
            
            extensions.ForEach(ext => Assert.True(Directory.GetFiles(Path.Join(pathToOutputDir, ext)).Length > 0));
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
        finally
        {
            Directory.Delete(pathToOutputDir,true);
        }
    }
}