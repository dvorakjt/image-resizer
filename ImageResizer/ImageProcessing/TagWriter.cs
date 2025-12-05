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
            var img = CreateImageElement(src, altText, srcset);
            pictureTagBuilder.Append(img);
        }

        pictureTagBuilder.Append("</picture>");
        return pictureTagBuilder.ToString();
    }

    public static string WriteTag(
        IImagePath outputPath,
        IEnumerable<ImageFileFormat> formats, 
        string altText,
        WidthsFormGroupValue widthsStrategyOptions
    )
    {
        var pictureTagBuilder = new StringBuilder("<picture>\n");
        
        if (formats.Contains(ImageFileFormat.AVIF))
        {
            var srcset = CreateSourceSet(outputPath, ImageFileFormat.AVIF, widthsStrategyOptions);
            var sizes = CreateSizes(widthsStrategyOptions);
            var source = CreateSourceElement(srcset, ImageFileFormat.AVIF.ToMimeType(), sizes);
            pictureTagBuilder.Append(source);
        }
        
        if (formats.Contains(ImageFileFormat.WebP))
        {
            var srcset = CreateSourceSet(outputPath, ImageFileFormat.WebP, widthsStrategyOptions);
            var sizes = CreateSizes(widthsStrategyOptions);
            var source = CreateSourceElement(srcset, ImageFileFormat.WebP.ToMimeType(), sizes);
            pictureTagBuilder.Append(source);
        }
        
        if (formats.Contains(ImageFileFormat.JPEG))
        {
            var srcset = CreateSourceSet(outputPath, ImageFileFormat.JPEG, widthsStrategyOptions);
            var sizes = CreateSizes(widthsStrategyOptions);
            var src = outputPath.GetURI(ImageFileFormat.JPEG, widthsStrategyOptions.DefaultImageWidth!.Value);
            var img = CreateImageElement(src, altText, srcset, sizes);
            pictureTagBuilder.Append(img);
        }

        pictureTagBuilder.Append("</picture>");
        return pictureTagBuilder.ToString();
    }

    public static string WriteTag(
        IImagePath outputPath, 
        IEnumerable<ImageFileFormat> formats, 
        string altText,
        MediaQueriesFormGroupValue 
            mediaQueriesStrategyOptions
    )
    {
        var pictureTagBuilder = new StringBuilder("<picture>\n");
        
        if (formats.Contains(ImageFileFormat.AVIF))
        {
            foreach (var mediaQuery in mediaQueriesStrategyOptions.MediaQueryAndImageWidths)
            {
                var uri = outputPath.GetURI(ImageFileFormat.AVIF, mediaQuery.ImageWidth!.Value);
                var formattedMediaQuery = FormatMediaQuery(mediaQuery.MediaQuery);
                var source = CreateSourceElement(uri, ImageFileFormat.AVIF.ToMimeType(), null, formattedMediaQuery);
                pictureTagBuilder.Append(source);
            }
        }
        
        if (formats.Contains(ImageFileFormat.WebP))
        {
            foreach (var mediaQuery in mediaQueriesStrategyOptions.MediaQueryAndImageWidths)
            {
                var uri = outputPath.GetURI(ImageFileFormat.WebP, mediaQuery.ImageWidth!.Value);
                var formattedMediaQuery = FormatMediaQuery(mediaQuery.MediaQuery);
                var source = CreateSourceElement(uri, ImageFileFormat.WebP.ToMimeType(), null, formattedMediaQuery);
                pictureTagBuilder.Append(source);
            }
        }
        
        if (formats.Contains(ImageFileFormat.JPEG))
        {
            foreach (var mediaQuery in mediaQueriesStrategyOptions.MediaQueryAndImageWidths)
            {
                var uri = outputPath.GetURI(ImageFileFormat.JPEG, mediaQuery.ImageWidth!.Value);
                var formattedMediaQuery = FormatMediaQuery(mediaQuery.MediaQuery);
                var source = CreateSourceElement(uri, ImageFileFormat.JPEG.ToMimeType(), null, formattedMediaQuery);
                pictureTagBuilder.Append(source);
            }
            
            var defaultImageSrc = outputPath.GetURI(ImageFileFormat.JPEG, mediaQueriesStrategyOptions.DefaultImageWidth!.Value);
            var img = CreateImageElement(defaultImageSrc, altText);
            pictureTagBuilder.Append(img);
        }

        pictureTagBuilder.Append("</picture>");
        return pictureTagBuilder.ToString();
    }

    private static string CreateSourceElement(string srcset, string mimeType, string? sizes = null, string? media = null)
    {
        var sourceTag = $"  <source srcset=\"{srcset}\" ";
        
        if (sizes != null)
        {
            sourceTag += $" sizes=\"{sizes}\" ";
        }

        if (media != null)
        {
            sourceTag += $" media=\"{media}\" ";
        }

        sourceTag += $"type=\"{mimeType}\" />\n";
        return sourceTag;
    }

    private static string CreateImageElement(string src, string altText, string? srcset = null, string? sizes = null)
    {
        var imgTag = $"  <img";
        if (srcset != null)
        {
            imgTag += $" srcset=\"{srcset}\"";
        }
        
        if (sizes != null)
        {
            imgTag += $" sizes=\"{sizes}\"";
        }
        
        imgTag += $" src=\"{src}\" alt=\"{altText}\" />\n";
        return imgTag;
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
    
    private static string CreateSourceSet(IImagePath outputPath, ImageFileFormat format,
        WidthsFormGroupValue widthsStrategyOptions)
    {
        var sources = widthsStrategyOptions.ScreenAndImageWidths.Select(screenAndImageWidths =>
        {
            return
                $"{outputPath.GetURI(format, screenAndImageWidths.ImageWidth!.Value)} {screenAndImageWidths.ImageWidth!.Value}w";
        }).ToList();
        
        sources.Add($"{outputPath.GetURI(format, widthsStrategyOptions.DefaultImageWidth!.Value)} {widthsStrategyOptions.DefaultImageWidth!.Value}w");
        return string.Join(", ", sources);
    }

    private static string CreateSizes(WidthsFormGroupValue widthsStrategyOptions)
    {
        var mediaQuery = widthsStrategyOptions.WidthThresholdsStrategy == WidthThresholdsStrategy.MaxWidths ? "max-width" :  "min-width";
        var sizes = widthsStrategyOptions.ScreenAndImageWidths.Select(screenAndImageWidths =>
        {
            return $"({mediaQuery}: {screenAndImageWidths.ScreenWidth}px) {screenAndImageWidths.ImageWidth!.Value}px";
        }).ToList();
        
        sizes.Add($"{widthsStrategyOptions.DefaultImageWidth!.Value}px");
        return string.Join(", ", sizes);
    }
    
    /*
        NetVips.Image.Resize rounds to the nearest pixel, so the calculation here corresponds to what you would get
        if you provided the multiplier directly to the NetVips.Image.Resize method.
    */
    private static int CalculateImageWidthFromDensity(int baseImageWidth, Density density)
    {
        return (int)Math.Round(baseImageWidth * density.ToMultiplier());
    }

    private static string FormatMediaQuery(string mediaQuery)
    {
        if (!IsEnclosedInParentheses(mediaQuery))
        {
            mediaQuery = $"({mediaQuery})";
        }
        
        return mediaQuery;
    }

    private static bool IsEnclosedInParentheses(string mediaQuery)
    {
        int openingParenthesesCount = 0;
		
        for(int i = 0; i < mediaQuery.Length - 1; i++)
        {
            if(mediaQuery[i] == '(') openingParenthesesCount++;
            else if(mediaQuery[i] == ')') openingParenthesesCount--;
            if(openingParenthesesCount <= 0) return false;
        }
		
        return true;
    }
}