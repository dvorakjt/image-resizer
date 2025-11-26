namespace ImageResizer.DataModel.ResponsiveImageSettings;

public record class DensitiesFormGroupValue
{
    public int? BaseImageWidth { get; init; }
    public int? DefaultImageWidth { get; init; }
    public IEnumerable<Density> Densities { get; init; }
}