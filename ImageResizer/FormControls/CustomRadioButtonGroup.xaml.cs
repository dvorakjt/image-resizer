using System.ComponentModel;
using ImageResizer.DataModel;
using Microsoft.Maui.Layouts;

namespace ImageResizer.FormControls;

public struct CustomRadioButtonGroupItem
{
    public string Content;
    public string Value;
}

public partial class CustomRadioButtonGroup : ContentView, IFormElement<string>, INotifyPropertyChanged
{
    public static BindableProperty JustifyContentProperty =
        BindableProperty.Create(nameof(JustifyContent), typeof(FlexJustify), typeof(TextInput), FlexJustify.Start);

    public FlexJustify JustifyContent
    {
        get => (FlexJustify)GetValue(JustifyContentProperty);
        set
        {
            SetValue(JustifyContentProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JustifyContent)));
        }
    }
    
    public event EventHandler<IFormElementState<string>>? StateChanged;
    public new event PropertyChangedEventHandler? PropertyChanged;

    public IFormElementState<string> State
    {
        get;
        private set
        {
            {
                field = value;
                StateChanged?.Invoke(this, field);
            }
        }
    }
    
    private CustomRadioButton _defaultRadioButton;
    
    public CustomRadioButtonGroup
    (
        IList<CustomRadioButtonGroupItem> items, 
        string defaultValue, 
        string groupName,
        double spacing = 0
    )
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
            var radioButton = new CustomRadioButton()
            {
                LabelText = item.Content,
                GroupName = groupName,
                IsChecked = item.Value == defaultValue,
                Margin = new Thickness(0, 0, spacing, 0)
            };

            if (item.Value == defaultValue)
            {
                _defaultRadioButton = radioButton;
            }

            radioButton.CheckedChanged += (object sender, CheckedChangedEventArgs e) =>
            {
                if (e.Value)
                {
                    State = new FormElementState<string>
                    {
                        Value = item.Value,
                        IsValid = true,
                        ErrorMessage = ""
                    };
                }
            };
            
            RadioButtonGroupLayout.Children.Add(radioButton);
        }
    
        State = new FormElementState<string>
        {
            Value = defaultValue,
            IsValid = true,
            ErrorMessage = ""
        };
    }
    
    public void Revalidate()
    {
        ;
    }

    public void DisplayErrors()
    {
        ;
    }

    public void Reset()
    {
        _defaultRadioButton.IsChecked = true;
    }
}