namespace ImageResizer.DataModel.ResponsiveImageSettings;

public record class MediaQueryAndImageWidth
{
    public string MediaQuery { get; set; }
    public int? ImageWidth { get; set; }
}