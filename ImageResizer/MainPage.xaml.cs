using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        
        
        var densitiesInput = new DensitiesInput();
        
        FormLayout.Children.Add(densitiesInput);
    }
   
}

