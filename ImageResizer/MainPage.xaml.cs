using ImageResizer.FormControls;
using ImageResizer.DataModel;
namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var textInput = TextInput.CreateNumeric("", (string v) => new ValidatorResult()
        {
            IsValid = !v.IsWhiteSpace(),
            ErrorMessage = v.IsWhiteSpace() ? "required" : ""
        }, int.MaxValue, false);
        
        MainLayout.Children.Add(textInput);

        var resetButton = new Button();
        resetButton.Text = "Reset";
        resetButton.Clicked += (sender, args) => textInput.Reset();
        
        var revealButton = new Button();
        revealButton.Text = "Reveal";
        revealButton.Clicked += (sender, e) => textInput.DisplayErrors();
        
        MainLayout.Children.Add(revealButton);
        MainLayout.Children.Add(resetButton);
    }
}