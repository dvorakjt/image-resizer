namespace ImageResizer.DataModel.Formats;

public record class WebPOptionsFormGroupValue
{
    public int? Quality { get; init; }
    public int? Effort { get; init; }
}