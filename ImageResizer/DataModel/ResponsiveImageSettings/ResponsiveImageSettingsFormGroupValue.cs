namespace ImageResizer.DataModel.ResponsiveImageSettings;

public record class ResponsiveImageSettingsFormGroupValue
{
    public ResponsiveImageStrategy ResponsiveImageStrategy { get; init; }
    public DensitiesFormGroupValue DensitiesStrategyOptions { get; init; }
    public WidthsFormGroupValue WidthsStrategyOptions { get; init; }
    public MediaQueriesFormGroupValue MediaQueriesStrategyOptions { get; init; }
}