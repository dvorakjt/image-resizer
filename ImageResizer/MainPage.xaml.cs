using System.Text.RegularExpressions;
using ImageResizer.Components;
using ImageResizer.Models;
using Microsoft.Maui.Layouts;
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
        var densitiesInput = new DensitiesInput();
        densitiesInput.StateChanged += (s, e) =>
        {
            Console.WriteLine(e.State.Value.BaseWidth);
            Console.WriteLine(e.State.IsValid);
            foreach (var density in e.State.Value.SelectedDensities)
            {
                Console.WriteLine(density);
            }

            Console.WriteLine();
        };

        FormLayout.Children.Add(densitiesInput);
    }
}

