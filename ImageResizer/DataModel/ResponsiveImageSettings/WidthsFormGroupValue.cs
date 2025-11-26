namespace ImageResizer.DataModel.ResponsiveImageSettings;

public record class WidthsFormGroupValue
{
    public WidthThresholdsStrategy WidthThresholdsStrategy { get; init; }
    public IEnumerable<ScreenAndImageWidths> ScreenAndImageWidths { get; init; }
    public int? DefaultImageWidth { get; init; }
}