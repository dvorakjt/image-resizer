using ImageResizer.Models;

namespace TestImageResizer.Models;

public class TestDensity
{
    [Fact]
    public void TestToHtmlString()
    {
        Assert.Equal("1x", Density.OneX.ToHtmlString());
        Assert.Equal("1.5x", Density.OneDot5X.ToHtmlString());
        Assert.Equal("2x", Density.TwoX.ToHtmlString());
        Assert.Equal("3x", Density.ThreeX.ToHtmlString());
        Assert.Equal("4x", Density.FourX.ToHtmlString());
    }
}