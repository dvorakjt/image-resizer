namespace ImageResizer.Models;

public enum Density
{
    OneX,
    OneDot5X,
    TwoX,
    ThreeX,
    FourX
}

public static class DensityExtensions
{
    public static string ToHtmlString(this Density density)
    {
        switch (density)
        {
            case Density.OneX:
                return "1x";
            case Density.OneDot5X:
                return "1.5x";
            case Density.TwoX:
                return "2x";
            case Density.ThreeX:
                return "3x";
            case Density.FourX:
                return "4x";
            default:
                throw new ArgumentOutOfRangeException(nameof(density), density, null);
        }
    }

    public static double ToMultiplier(this Density density)
    {
        switch (density)
        {
            case Density.OneX:
                return 1;
            case Density.OneDot5X:
                return 1.5;
            case Density.TwoX:
                return 2;
            case Density.ThreeX:
                return 3;
            case Density.FourX:
                return 4;
            default:
                throw new ArgumentOutOfRangeException(nameof(density), density, null);
        }
    }
}