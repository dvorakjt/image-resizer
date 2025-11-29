using System.Text;
using ImageResizer.DataModel.Formats;
using ImageResizer.DataModel.ResponsiveImageSettings;

namespace ImageResizer.ImageProcessing;

public static class TagWriter
{
    public static string WriteTag(
        IImagePath outputPath, 
        IEnumerable<ImageFileFormat> formats, 
        string altText,
        DensitiesFormGroupValue densitiesStrategyOptions
    )
    {
        var pictureTagBuilder = new StringBuilder("<picture>\n");
        
        if (formats.Contains(ImageFileFormat.AVIF))
        {
            var srcset = CreateSourceSet(outputPath, ImageFileFormat.AVIF, densitiesStrategyOptions);
            var source = CreateSourceElement(srcset, ImageFileFormat.AVIF.ToMimeType());
            pictureTagBuilder.Append(source);
        }
        
        if (formats.Contains(ImageFileFormat.WebP))
        {
            var srcset = CreateSourceSet(outputPath, ImageFileFormat.WebP, densitiesStrategyOptions);
            var source = CreateSourceElement(srcset, ImageFileFormat.WebP.ToMimeType());
            pictureTagBuilder.Append(source);
        }
        
        if (formats.Contains(ImageFileFormat.JPEG))
        {
            var srcset = CreateSourceSet(outputPath, ImageFileFormat.JPEG, densitiesStrategyOptions);
            var src = outputPath.GetURI(ImageFileFormat.JPEG, densitiesStrategyOptions.DefaultImageWidth!.Value);
            var img = CreateImageElement(srcset, src, altText);
            pictureTagBuilder.Append(img);
        }

        pictureTagBuilder.Append("</picture>");
        return pictureTagBuilder.ToString();
    }

    public static string WriteTag(IImagePath outputPath, IEnumerable<ImageFileFormat> formats, WidthsFormGroupValue widthsStrategyOptions)
    {
        throw new NotImplementedException();
    }

    public static string WriteTag(IImagePath outputPath, IEnumerable<ImageFileFormat> formats, MediaQueriesFormGroupValue mediaQueriesStrategyOptions)
    {
        throw new NotImplementedException();
    }

    private static string CreateSourceElement(string srcset, string mimeType)
    {
        return $"  <source srcset=\"{srcset}\" type=\"{mimeType}\" />\n";
    }

    private static string CreateImageElement(string srcset, string src, string altText)
    {
        return $"  <img srcset=\"{srcset}\" src=\"{src}\" alt=\"{altText}\" />\n";
    }

    private static string CreateSourceSet(IImagePath outputPath, ImageFileFormat format,
        DensitiesFormGroupValue densitiesStrategyOptions)
    {
        var sources = densitiesStrategyOptions.Densities.OrderBy(d => d.ToMultiplier()).Select(density =>
        {
            var imageWidth = CalculateImageWidthFromDensity(densitiesStrategyOptions.BaseImageWidth!.Value, density);
            var uri = outputPath.GetURI(format, imageWidth);
            var source = $"{uri} {density.ToHtmlString()}";
            return source;
        }).ToList();
        
        sources.Add(outputPath.GetURI(format, densitiesStrategyOptions.DefaultImageWidth!.Value));
        
        return string.Join(", ", sources);
    }
    
    /*
        NetVips.Image.Resize rounds to the nearest pixel, so the calculation here corresponds to what you would get
        if you provided the multiplier directly to the NetVips.Image.Resize method.
    */
    private static int CalculateImageWidthFromDensity(int baseImageWidth, Density density)
    {
        return (int)Math.Round(baseImageWidth * density.ToMultiplier());
    }
}