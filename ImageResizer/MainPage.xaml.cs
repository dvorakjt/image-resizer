using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        InitializeFormElements();
    }
    
    // REMEMBER -> VALIDATE DENSITIES WIDTH --> should be 5000 or less
    private void InitializeFormElements()
    {
        var widthsInput = new WidthsInput();

        widthsInput.StateChanged += (sender, args) =>
        {
            Console.WriteLine(args.State.Value.DefaultWidth);
            Console.WriteLine(args.State.Value.WidthComparisonMode);
            Console.WriteLine(args.State.IsValid);
        };
        
        FormLayout.Children.Add(widthsInput);
    }
}

