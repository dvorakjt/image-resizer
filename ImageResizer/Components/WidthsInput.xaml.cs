using ImageResizer.Models;

namespace ImageResizer.Components;

public struct ScreenAndImageWidths : IComparable<ScreenAndImageWidths>
{
    public required int ScreenWidth { get; init; }
    public int? ImageWidth { get; init; }
    
    public int CompareTo(ScreenAndImageWidths other)
    {
        return this.ScreenWidth.CompareTo(other.ScreenWidth);
    }
}

public struct WidthsInputValue
{
    public required WidthComparisonMode  WidthComparisonMode { get; init; }
    public int? DefaultWidth { get; init; }
    public required IEnumerable<ScreenAndImageWidths> ScreenAndImageWidthsList { get; init; }
}

public partial class WidthsInput : ContentView
{
    // need three controls: one for mode, one for adding a width, one for editing widths
    // the edited widths can be deleted, neither duplicate nor empty widths can be added, empty 
    // width errors will be revealed when RevealErrors is called, screen widths are sorted depending on the mode
    
    public WidthsInput()
    {
        InitializeComponent();

        var modeInput = new RadioButtonGroup(
        [
            new RadioButtonGroupItem()
            {
                Content = "Max-Widths",
                Value = WidthComparisonMode.LTE.ToString()
            },
            new RadioButtonGroupItem()
            {
                Content = "Min-Widths",
                Value = WidthComparisonMode.GTE.ToString()
            },

        ], WidthComparisonMode.LTE.ToString(), "WidthComparisonModeGroup");
    }
}