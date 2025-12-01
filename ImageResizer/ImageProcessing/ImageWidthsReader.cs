using ImageResizer.DataModel.ResponsiveImageSettings;

namespace ImageResizer.ImageProcessing;

public static class ImageWidthsReader
{
    public static HashSet<int> GetImageWidths(DensitiesFormGroupValue densitiesStrategyOptions)
    {
        var imageWidths = new HashSet<int>();
        if (!densitiesStrategyOptions.BaseImageWidth.HasValue)
        {
            throw new ArgumentException("densitiesStrategyOptions.BaseImageWidth cannot be null.");
        }
        
        foreach (var density in densitiesStrategyOptions.Densities)
        {
            var adjustedWidth = (int)Math.Round(densitiesStrategyOptions.BaseImageWidth.Value * density.ToMultiplier());
            imageWidths.Add(adjustedWidth);
        }
        
        if (!densitiesStrategyOptions.DefaultImageWidth.HasValue)
        {
            throw new ArgumentException("densitiesStrategyOptions.DefaultImageWidth cannot be null.");
        }
        
        imageWidths.Add(densitiesStrategyOptions.DefaultImageWidth.Value);
        return imageWidths;
    }

    public static HashSet<int> GetImageWidths(WidthsFormGroupValue widthsStrategyOptions)
    {
        var imageWidths = new HashSet<int>();
        
        if (!widthsStrategyOptions.DefaultImageWidth.HasValue)
        {
            throw new ArgumentException("widthsStrategyOptions.DefaultImageWidth cannot be null.");
        }
        
        imageWidths.Add(widthsStrategyOptions.DefaultImageWidth.Value);

        foreach (var screenAndImageWidth in widthsStrategyOptions.ScreenAndImageWidths)
        {
            if (!screenAndImageWidth.ImageWidth.HasValue)
            {
                throw new ArgumentException("widthsStrategyOptions.ScreenAndImageWidths contains a null image width.");
            }
            
            imageWidths.Add(screenAndImageWidth.ImageWidth.Value);
        }
        
        return imageWidths;
    }

    public static HashSet<int> GetImageWidths(MediaQueriesFormGroupValue mediaQueriesStrategyOptions)
    {
        var imageWidths = new HashSet<int>();
        
        if (!mediaQueriesStrategyOptions.DefaultImageWidth.HasValue)
        {
            throw new ArgumentException("mediaQueriesStrategyOptions.DefaultImageWidth cannot be null.");
        }
        
        imageWidths.Add(mediaQueriesStrategyOptions.DefaultImageWidth.Value);

        foreach (var mediaQueryAndImageWidth in mediaQueriesStrategyOptions.MediaQueryAndImageWidths)
        {
            if (!mediaQueryAndImageWidth.ImageWidth.HasValue)
            {
                throw new ArgumentException("mediaQueriesStrategyOptions.MediaQueryAndImageWidths contains a null image width.");
            }
            
            imageWidths.Add(mediaQueryAndImageWidth.ImageWidth.Value);
        }
        
        return imageWidths;
    }
}