namespace ImageResizer.DataModel.Output;

public record class OutputFormGroupValue
{
    public string Filename { get; init; }
    public string VersionId { get; init; }
    public string PathToPublicDirectory { get; init; }
    public string PathFromPublicDirectory { get; init; }
}