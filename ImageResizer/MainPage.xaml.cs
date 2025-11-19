using ImageResizer.FormControls;
using ImageResizer.DataModel;

namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();

        var heading = new Label()
        {
            Text = "Heading",
            StyleClass = ["Heading"]
        };
        
        MainLayout.Children.Add(heading);

        var subheading = new Label()
        {
            Text = "Subheading1",
            StyleClass = ["Subheading1"]
        };
        
        MainLayout.Children.Add(subheading);

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

        var textInput = new TextInputBuilder().WithLabel("Text Input").WithValidator(FormControlHelpers.CreateRequiredFieldValidator("Required")).Build();
        MainLayout.Children.Add(textInput);
        
        var checkboxGroup =  new CustomCheckboxGroup(
            [
                new CheckboxGroupItem
                {
                    Value = Density.OneX.ToHtmlString(), 
                    Label = Density.OneX.ToHtmlString(),
                    IsChecked = true, 
                    IsFrozen = true
                },
                new CheckboxGroupItem
                {
                    Value = Density.OneDot5X.ToHtmlString(), 
                    Label = Density.OneDot5X.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.TwoX.ToHtmlString(), 
                    Label = Density.TwoX.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.ThreeX.ToHtmlString(), 
                    Label = Density.ThreeX.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.FourX.ToHtmlString(), 
                    Label = Density.FourX.ToHtmlString(),
                    IsChecked = true, 
                },
            ]
        )
        {
            LabelText = "Densities",
        };
        
        MainLayout.Children.Add(checkboxGroup);
    }
}