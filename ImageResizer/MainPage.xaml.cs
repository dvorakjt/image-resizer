using ImageResizer.FormControls;
using ImageResizer.DataModel;
namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var textInput = new TextInputBuilder().Build();
        var withLabel = new TextInputBuilder().WithLabel("Hello World").NumericAllowZero().Build();
        
        MainLayout.Children.Add(textInput);
        MainLayout.Children.Add(withLabel);
    }
}