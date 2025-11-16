using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        var widthsInput = new WidthsInput();

        widthsInput.StateChanged += (sender, e) =>
        {
            Console.WriteLine(e.State.IsValid);
        };
        
        FormLayout.Children.Add(widthsInput);
    }
   
}

