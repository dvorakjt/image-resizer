using System.ComponentModel;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public struct RadioButtonGroupItem
{
    public string Content;
    public string Value;
}

public partial class RadioButtonGroup : ContentView, IFormElement<string>, INotifyPropertyChanged
{
    public static BindableProperty LabelTextProperty =
        BindableProperty.Create("LabelText", typeof(string), typeof(TextInput), "");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set {
            SetValue(LabelTextProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }
    
    public event EventHandler<FormElementStateChangedEventArgs<string>>? StateChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public FormElementState<string> State
    {
        get;
        private set
        {
            {
                field = value;
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<string>(field));
            }
        }
    }

    public RadioButtonGroup(IList<RadioButtonGroupItem> items, string defaultValue, string groupName)
    {
        if (items.Count() == 0)
        {
            throw new ArgumentException("Items cannot be an empty collection.", nameof(items));
        }

        if (!items.Any(item => item.Value == defaultValue))
        {
            throw new ArgumentException("Item values must include the default value.", nameof(defaultValue));
        }

        InitializeComponent();

        foreach (var item in items)
        {
            var radioButton = new RadioButton()
            {
                Content = item.Content,
                Value = item.Value,
                GroupName = groupName,
                IsChecked = item.Value == defaultValue
            };

            radioButton.CheckedChanged += (object sender, CheckedChangedEventArgs e) =>
            {
                State = new FormElementState<string>
                {
                    Value = item.Value,
                    IsValid = true
                };
            };
            
            RadioButtonGroupLayout.Children.Add(radioButton);
        }
    
        State = new FormElementState<string>
        {
            Value = defaultValue,
            IsValid = true
        };
    }
}