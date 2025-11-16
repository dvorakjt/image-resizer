using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        FormLayout.Children.Add(new WidthsInput());
    }
   
}

