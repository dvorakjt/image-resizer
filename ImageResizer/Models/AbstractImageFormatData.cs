namespace ImageResizer.Models;

public class AbstractImageFormatData
{
    public int Quality { get; }

    protected AbstractImageFormatData(int quality)
    {
        Quality = quality;
    }
}