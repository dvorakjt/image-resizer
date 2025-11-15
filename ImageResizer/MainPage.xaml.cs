using ImageResizer.Components;
using ImageResizer.Models;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        InitializeFormElements();
    }
    
    private void InitializeFormElements()
    {
        var altTextInput = new TextInput("", value =>
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            string message = isValid ? "" : "Please enter some alt text.";
            return new ValidatorFuncResult(
                isValid,
                message);
        })
        {
            LabelText = "Alt Text",
        };

        altTextInput.StateChanged += (object sender, FormElementStateChangedEventArgs<string> e) =>
        {
            Console.WriteLine(e.State.Value);
        };
    
        FormLayout.Children.Add(altTextInput);
    }
}

