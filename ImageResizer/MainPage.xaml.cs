using ImageResizer.ViewModels;
using ImageResizer.Views;
using RadioButtonGroup = ImageResizer.Views.RadioButtonGroup;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();

        var imagePicker = new ImagePicker()
        {
           Margin = new Thickness(0,0,0,20),
        };
        
        FormLayout.Children.Add(imagePicker);
        
        var altTextInput = new TextInput
        (
            "",
            FormElementHelpers.CreateRequiredFieldValidator("Please enter some alt text")
        )
        {
            LabelText = "Alt Text",
            MinimumWidthRequest = AppDimensions.CONTENT_WIDTH,
            MaximumWidthRequest = AppDimensions.CONTENT_WIDTH,
            Margin = new Thickness(0,0,0,20),
        };
        
        FormLayout.Children.Add(altTextInput);

        var modeInput = new RadioButtonGroup([
            new RadioButtonGroupItem()
            {
                Content = "Densities",
                Value = ResponsivenessMode.Densities.ToString(),
            },
            new RadioButtonGroupItem() {
                Content = "Widths",
                Value = ResponsivenessMode.Widths.ToString()
            },
            new RadioButtonGroupItem()
            {
                Content = "MediaQueries",
                Value = ResponsivenessMode.MediaQueries.ToString(),
            }
        ], ResponsivenessMode.Densities.ToString(), "ResponsivenessMode");
        
        FormLayout.Children.Add(modeInput);

        var densities = new DensitiesInput();
        FormLayout.Children.Add(densities);
    }
   
}

