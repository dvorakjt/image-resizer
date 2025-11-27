namespace ImageResizer.DataModel.ResponsiveImageSettings;

public class MediaQueriesFormGroupValue
{
    public IEnumerable<MediaQueryAndImageWidth> MediaQueryAndImageWidths { get; init; }
    public int? DefaultImageWidth { get; init; }
}