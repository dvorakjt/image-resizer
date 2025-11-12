namespace ImageResizer.Models;

public class WebPImageFormatData : AbstractImageFormatData
{
    public int Effort { get; }

    public WebPImageFormatData(int quality, int effort) : base(quality)
    {
        Effort = effort;
    }

    public override string GetExtension()
    {
        return ".webp";
    }

    public override string GetMimeType()
    {
        return "image/webp";
    }
}