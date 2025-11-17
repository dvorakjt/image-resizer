using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();

        var imagePicker = new ImagePicker();
        var densitiesInput = new DensitiesInput();
        var mediaQueriesInput = new MediaQueriesInput();
        
        var avifOptionsInput = new QualityAndEffortInput((0, 100), (0, 9), 50, 4)
        {
            LabelText = "AVIF Options"
        };
        
        var webPOptionsInput = new QualityAndEffortInput((0, 100), (0, 6), 75, 4)
        {
            LabelText = "WebP Options"
        };

        var widthsInput = new WidthsInput();
        
        FormLayout.Children.Add(imagePicker);
        FormLayout.Children.Add(densitiesInput);
        FormLayout.Children.Add(mediaQueriesInput);
        FormLayout.Children.Add(avifOptionsInput);
        FormLayout.Children.Add(webPOptionsInput);
        FormLayout.Children.Add(widthsInput);
    }
   
}

