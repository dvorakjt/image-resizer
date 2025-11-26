using ImageResizer.DataModel.ResponsiveImageSettings;

namespace ImageResizer.ImageProcessing;

/// <summary>
/// Reads distinct image widths from form group values so that the image can be resized accordingly.
/// </summary>
public interface IImageWidthsReader
{
    HashSet<int> GetImageWidths(DensitiesFormGroupValue densitiesStrategyOptions);
    HashSet<int> GetImageWidths(WidthsFormGroupValue widthsStrategyOptions);
    HashSet<int> GetImageWidths(MediaQueriesFormGroupValue mediaQueriesStrategyOptions);
}