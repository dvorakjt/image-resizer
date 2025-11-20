namespace ImageResizer.DataModel;

public class ScreenAndImageWidths : IComparable<ScreenAndImageWidths>
{
    public int ScreenWidth { get; init; }
    public int? ImageWidth { get; set; }

    public int CompareTo(ScreenAndImageWidths other)
    {
        return ScreenWidth.CompareTo(other.ScreenWidth);
    }
}