using ImageResizer.ViewModels;
using ImageResizer.Views;

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
        };
        
        FormLayout.Children.Add(altTextInput);
    }
   
}

