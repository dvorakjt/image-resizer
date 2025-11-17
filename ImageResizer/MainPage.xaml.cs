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

        var mq = new MediaQueriesInput();
        FormLayout.Children.Add(mq);

        var qualityAndEffortInput = new QualityAndEffortInput
        (
            (0, 100),
            (0, 9),
            50,
            4
        )
        {
            LabelText = "AVIF Options"
        };
        
        FormLayout.Children.Add(qualityAndEffortInput);

        // var imagePicker = new ImagePicker()
        // {
        //    Margin = new Thickness(0,0,0,10),
        // };
        //
        // FormLayout.Children.Add(imagePicker);
        //
        // var altTextInput = new TextInput
        // (
        //     "",
        //     FormElementHelpers.CreateRequiredFieldValidator("Please enter some alt text")
        // )
        // {
        //     LabelText = "Alt Text",
        //     MinimumWidthRequest = AppDimensions.CONTENT_WIDTH,
        //     MaximumWidthRequest = AppDimensions.CONTENT_WIDTH,
        //     Margin = new Thickness(0,0,0,20),
        // };
        //
        // FormLayout.Children.Add(altTextInput);
        //
        // var modeHeading = new Label()
        // {
        //     Text = "Mode",
        //     StyleClass = ["SubHeading"],
        //     Margin = new Thickness(0, 0, 0, 10),
        //     HorizontalOptions = LayoutOptions.Start
        // };
        //
        // FormLayout.Children.Add(modeHeading);
        //
        // var modeInput = new RadioButtonGroup([
        //     new RadioButtonGroupItem()
        //     {
        //         Content = "Densities",
        //         Value = ResponsivenessMode.Densities.ToString(),
        //     },
        //     new RadioButtonGroupItem()
        //     {
        //         Content = "Widths",
        //         Value = ResponsivenessMode.Widths.ToString()
        //     },
        //     new RadioButtonGroupItem()
        //     {
        //         Content = "Media Queries",
        //         Value = ResponsivenessMode.MediaQueries.ToString(),
        //     }
        // ], ResponsivenessMode.Densities.ToString(), "ResponsivenessMode")
        // {
        //     MinimumWidthRequest = AppDimensions.CONTENT_WIDTH,
        //     MaximumWidthRequest = AppDimensions.CONTENT_WIDTH,
        //     Margin = new Thickness(0, 0, 0, 20),
        // };
        //
        // FormLayout.Children.Add(modeInput);
        //
        // var densities = new DensitiesInput()
        // {
        //     Margin = new Thickness(0, 0, 0, 20),
        // };
        // FormLayout.Children.Add(densities);
        //
        /*var widths = new WidthsInput()
        {
            Margin = new Thickness(0, 0, 0, 20),
        };
        FormLayout.Children.Add(widths);*/
    }

    private void SetFormWidth()
    {
        FormLayout.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        FormLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
    }
}

