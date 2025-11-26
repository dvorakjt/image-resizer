namespace ImageResizer.DataModel.ResponsiveImageSettings;

public record class MediaQueriesFormGroupValue
{
    public IEnumerable<MediaQueryAndImageWidth> MediaQueryAndImageWidths { get; init; }
    public int? DefaultImageWidth { get; init; }
}