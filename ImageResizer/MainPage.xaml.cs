using ImageResizer.FormControls;
using ImageResizer.DataModel;

namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();

        var responsivenessModesInput = new CustomRadioButtonGroup
        (
            [
                new RadioButtonGroupItem()
                {
                    Content = "Densities",
                    Value = ImageSwitchingMode.Densities.ToString()
                },
                new RadioButtonGroupItem()
                {
                    Content = "Widths",
                    Value = ImageSwitchingMode.Widths.ToString()
                },
                new RadioButtonGroupItem()
                {
                    Content = "Media Queries",
                    Value = ImageSwitchingMode.MediaQueries.ToString()
                }
            ],
            ImageSwitchingMode.Densities.ToString(),
            "ImageSwitchingMode"
        );
        
        MainLayout.Children.Add(responsivenessModesInput);
    }
}