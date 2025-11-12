namespace ImageResizer.Models;

public struct MediaQueryWithImageWidth
{
    public string MediaQuery { get; }
    public int ImageWidth { get; }

    public MediaQueryWithImageWidth(string mediaQuery, int imageWidth)
    {
        MediaQuery = mediaQuery;
        ImageWidth = imageWidth;
    }
}