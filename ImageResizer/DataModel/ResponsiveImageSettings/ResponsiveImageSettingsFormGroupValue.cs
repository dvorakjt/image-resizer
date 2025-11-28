namespace ImageResizer.DataModel.ResponsiveImageSettings;

public class ResponsiveImageSettingsFormGroupValue
{
    public ResponsiveImageStrategy ResponsiveImageStrategy { get; init; }
    public DensitiesFormGroupValue DensitiesStrategyOptions { get; init; }
    public WidthsFormGroupValue WidthsStrategyOptions { get; init; }
    public MediaQueriesFormGroupValue MediaQueriesStrategyOptions { get; init; }
}