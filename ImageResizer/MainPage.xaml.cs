using System.Text.RegularExpressions;
using ImageResizer.Components;
using ImageResizer.Models;
using RadioButtonGroup = ImageResizer.Components.RadioButtonGroup;

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
    
        FormLayout.Children.Add(altTextInput);

        var radioGroup = new RadioButtonGroup([
            new RadioButtonGroupItem() { Content = "Dogs", Value = "dogs" },
            new RadioButtonGroupItem() { Content = "Cats", Value = "cats" },
            new RadioButtonGroupItem() { Content = "Baboons", Value = "baboons" },
        ], "dogs", "animals")
        {
            LabelText = "Animals",
        };
        
        radioGroup.StateChanged += (object sender, FormElementStateChangedEventArgs<string> e) =>
        {
            Console.WriteLine(e.State.Value);
        };
        
        FormLayout.Add(radioGroup);

        var numericInput = new TextInput("", FormElementHelpers.CreateRequiredFieldValidator("Please enter a number."),
            FormElementHelpers.AllowOnlyDigits)
        {
            LabelText = "Numeric Input",
        };
        
        numericInput.SetValue("33");
        
        FormLayout.Children.Add(numericInput);
    }
}

