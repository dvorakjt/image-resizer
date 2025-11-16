using HtmlAgilityPack;
using ImageResizer.Models;

namespace TestImageResizer.Models;

public class TestDensitiesModeFormData
{
    [Fact]
    public async Task TestDensitiesModeFormDataSave()
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

            var baseImageWidth = 50;
            var densities = new List<Density>();
            densities.Add(Density.OneX);
            densities.Add(Density.OneDot5X);
            densities.Add(Density.TwoX);
            densities.Add(Density.ThreeX);
            densities.Add(Density.FourX);

            var formData = new DensitiesModeFormData(
                bytes,
                filename,
                versionNumber,
                pathToPublicDir,
                pathFromPublicDir,
                altText,
                imageFormats,
                baseImageWidth,
                densities);

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