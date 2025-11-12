namespace ImageResizer.Models;

public class WebPImageFormatData : AbstractImageFormatData
{
    public int Effort { get; }

    public WebPImageFormatData(int quality, int effort) : base(quality)
    {
        Effort = effort;
    }
}