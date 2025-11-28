namespace ImageResizer.DataModel.ResponsiveImageSettings;

public class DensitiesFormGroupValue
{
    public int? BaseImageWidth { get; init; }
    public int? DefaultImageWidth { get; init; }
    public IEnumerable<Density> Densities { get; init; }
}