using ImageResizer.ViewModels;
using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();

        var imagePicker = new ImagePicker();
        var altTextInput = new TextInput
        (
            "",
            FormElementHelpers.CreateRequiredFieldValidator("Please enter some alt text")
        )
        {
            LabelText = "Alt Text"
        };
        
        FormLayout.Children.Add(altTextInput);

        var custom = new CustomEntry();
        FormLayout.Children.Add(custom);
    }
   
}

