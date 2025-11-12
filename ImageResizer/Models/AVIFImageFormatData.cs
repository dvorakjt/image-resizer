namespace ImageResizer.Models;

public class AVIFImageFormatData : AbstractImageFormatData
{
    public int Effort { get; }
    
    public AVIFImageFormatData(int quality, int effort) : base(quality)
    {
        Effort = effort;
    }
}