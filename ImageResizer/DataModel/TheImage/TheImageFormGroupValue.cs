namespace ImageResizer.DataModel.TheImage;

public record class TheImageFormGroupValue
{
    public Stream? ImageStream { get; init; }
    public string AltText { get; init; }
}