using ImageResizer.Components;

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
        var altTextInput = new TextInput(value =>
        {
            bool isValid = value.Trim().Length > 0;
            string message = isValid ? "" : "Please enter some alt text.";
            return new ValidationResult(
                isValid ? Validity.Valid : Validity.Invalid,
                message);
        })
        {
            LabelText = "Alt Text",
        };

        altTextInput.StateChanged += OnStateChanged;
    
        FormLayout.Children.Add(altTextInput);
    }

    private void OnStateChanged(object sender, FormElementEventArgs<string> e)
    {
        Console.WriteLine(e.Validity);
        Console.WriteLine(e.Value);
    }
}

