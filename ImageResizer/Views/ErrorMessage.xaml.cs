using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Views;

public partial class ErrorMessage : ContentView
{
    public static BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(ErrorMessage), "");
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public ErrorMessage()
    {
        InitializeComponent();
    }
}