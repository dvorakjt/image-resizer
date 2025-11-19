using System.ComponentModel;

namespace ImageResizer.FormControls;

public partial class CustomCheckbox : ContentView, INotifyPropertyChanged
{
    public static BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CustomCheckbox), false);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set {
            SetValue(IsCheckedProperty, value); 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(value));
        }
    }
    
    public static BindableProperty LabelTextProperty =
        BindableProperty.Create("LabelText", typeof(string), typeof(CustomCheckbox), "");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set {
            SetValue(LabelTextProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }
    
    public static BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(CustomCheckbox), true);

    public bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set {
            SetValue(IsEnabledProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextBorderAndCheckmarkColor)));
        }
    }

    public Color TextBorderAndCheckmarkColor
    {
        get => IsEnabled ? Color.Parse("Black") : Color.Parse("Gray");
    }
    
    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public CustomCheckbox()
    {
        InitializeComponent();
    }

    private void OnTapped(object sender, TappedEventArgs args)
    {
        if (IsEnabled)
        {
            IsChecked = !IsChecked;
        }
    }
}