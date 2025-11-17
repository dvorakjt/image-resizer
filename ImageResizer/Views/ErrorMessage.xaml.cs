using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    
    public static BindableProperty IsVisibleProperty =
        BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ErrorMessage), false);
    
    public bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }
    
    public ErrorMessage()
    {
        InitializeComponent();
    }
}