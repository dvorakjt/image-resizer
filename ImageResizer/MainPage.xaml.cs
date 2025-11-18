using ImageResizer.FormControls;
using ImageResizer.DataModel;
namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var textInput = TextInput.Create("", (string v) => new ValidatorResult()
        {
            IsValid = !v.IsWhiteSpace(),
            ErrorMessage = v.IsWhiteSpace() ? "required" : ""
        });
        MainLayout.Children.Add(textInput);
    }
}