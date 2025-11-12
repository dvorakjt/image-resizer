namespace ImageResizer.Models;

public abstract class AbstractImageFormatData
{
    public int Quality { get; }

    protected AbstractImageFormatData(int quality)
    {
        Quality = quality;
    }

    public virtual string GetExtension()
    {
        return ".jpg";
    }

    public virtual string GetMimeType()
    {
        return "image/jpeg";
    }
}