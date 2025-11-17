using ImageResizer.ViewModels;
using ImageResizer.Views;
using RadioButtonGroup = ImageResizer.Views.RadioButtonGroup;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        SetFormWidth();

        var imagePicker = new ImagePicker()
        {
           Margin = new Thickness(0,0,0,10),
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

        var modeHeading = new Label()
        {
            Text = "Mode",
            StyleClass = ["SubHeading"],
            Margin = new Thickness(0, 0, 0, 10),
            HorizontalOptions = LayoutOptions.Start
        };
        
        FormLayout.Children.Add(modeHeading);

        var modeInput = new RadioButtonGroup([
            new RadioButtonGroupItem()
            {
                Content = "Densities",
                Value = ResponsivenessMode.Densities.ToString(),
            },
            new RadioButtonGroupItem()
            {
                Content = "Widths",
                Value = ResponsivenessMode.Widths.ToString()
            },
            new RadioButtonGroupItem()
            {
                Content = "Media Queries",
                Value = ResponsivenessMode.MediaQueries.ToString(),
            }
        ], ResponsivenessMode.Densities.ToString(), "ResponsivenessMode")
        {
            MinimumWidthRequest = AppDimensions.CONTENT_WIDTH,
            MaximumWidthRequest = AppDimensions.CONTENT_WIDTH,
            Margin = new Thickness(0, 0, 0, 20),
        };
        
        FormLayout.Children.Add(modeInput);

        var densities = new DensitiesInput();
        FormLayout.Children.Add(densities);

        var radio = new IRRadioButton()
        {
            IsChecked = true,
            LabelText = "My Radio",
            Margin = new Thickness(20)
        };
        
        FormLayout.Add(radio);
    }

    private void SetFormWidth()
    {
        FormLayout.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        FormLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
    }
}

