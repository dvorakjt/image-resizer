using ImageResizer.Models;
using HtmlAgilityPack;

namespace TestImageResizer;

public class TestMediaQueryModeFormData
{
    [Fact]
    public async Task TestMediaQueryModeFormDataSave()
    {
        var filename = Guid.NewGuid().ToString();
        var pathToPublicDir = Environment.CurrentDirectory;
        var pathFromPublicDir = "temp";
        var pathToOutputDir = Path.Join(pathToPublicDir, pathFromPublicDir, filename);
        
        try
        {
            using var originalImage = NetVips.Image.Black(1200, 1200);
            var bytes = originalImage.WriteToBuffer(".jpg");
            var versionNumber = 1;
            var altText = "A black square";

            var imageFormats = new List<AbstractImageFormatData>();
            imageFormats.Add(new AVIFImageFormatData(100, 1));
            imageFormats.Add(new WebPImageFormatData(100, 1));
            imageFormats.Add(new JPGImageFormatData(100));

            var mediaQueries = new List<MediaQueryWithImageWidth>();
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 100px) and (max-resolution: 1x)", 100));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 150px) and (max-resolution: 1.5x)", 150));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 200px) and (max-resolution: 2x)", 200));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 300px) and (max-resolution: 3x)", 300));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 400px) and (max-resolution: 4x)", 400));
            
            // Simulate CSS rule that changes image width on larger screens
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 400px) and (max-resolution: 1x)", 300));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 600px) and (max-resolution: 1.5x)", 450));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 800px) and (max-resolution: 2x)", 600));
            mediaQueries.Add(new MediaQueryWithImageWidth("(max-width: 1200px) and (max-resolution: 3x)", 900));
            
            var formData = new MediaQueryModeFormData(
                bytes,
                filename,
                versionNumber,
                pathToPublicDir,
                pathFromPublicDir,
                altText,
                imageFormats,
                originalImage.Width,
                mediaQueries);

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