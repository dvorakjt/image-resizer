using System.ComponentModel;
using ImageResizer.ViewModels;
using Microsoft.Maui.Layouts;

namespace ImageResizer.Views;

public struct CheckboxGroupItem
{
    public required string Label { get; init; }
    public required string Value { get; init; }
    public bool IsChecked { get; init; }
    public bool IsFrozen { get; init; }
}

public partial class CheckboxGroup : ContentView, IFormElement<IEnumerable<string>>, INotifyPropertyChanged
{
    public static BindableProperty DirectionProperty = BindableProperty.Create(nameof(Direction), typeof(FlexDirection), typeof(CheckboxGroup), FlexDirection.Column);

    public FlexDirection Direction
    {
        get => (FlexDirection)GetValue(DirectionProperty);
        set {
            SetValue(DirectionProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Direction)));
        }
    }
    
    public static BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(TextInput), "");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set {
            SetValue(LabelTextProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }
    
    public event EventHandler<FormElementStateChangedEventArgs<IEnumerable<string>>>? StateChanged;
    public event EventHandler<PropertyChangedEventArgs> PropertyChanged;
    
    public FormElementState<IEnumerable<string>> State { get; private set
    {
        field = value;
        StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<IEnumerable<string>>(field));
    } }
    
    public CheckboxGroup(IEnumerable<CheckboxGroupItem> items)
    {
        if (items.Count() == 0)
        {
            throw new ArgumentException("Items cannot be empty.");
        }
        
        InitializeComponent();
        InitializeInputElements(items);
    }

    private void InitializeInputElements(IEnumerable<CheckboxGroupItem> items)
    {
        IList<string> selectedItems = new List<string>();
        
        foreach (var item in items)
        {
            var container = new HorizontalStackLayout();

            var checkbox = new IRCheckbox()
            {
                LabelText = item.Label,
                IsChecked = item.IsChecked,
                IsEnabled = !item.IsFrozen
            };

            if (item.IsChecked)
            {
                selectedItems.Add(item.Value);
            }

            checkbox.CheckedChanged += (sender, e) =>
            {
                var newSelectedItems = new List<string>(State.Value);

                if (e.Value && !newSelectedItems.Contains(item.Value))
                {
                    newSelectedItems.Add(item.Value);
                }
                else if (!e.Value && newSelectedItems.Contains(item.Value))
                {
                    newSelectedItems.Remove(item.Value);
                }

                State = new FormElementState<IEnumerable<string>>()
                {
                    Value = newSelectedItems,
                    IsValid = true
                };
            };
            
            CheckboxesLayout.Children.Add(checkbox);
        }

        State = new FormElementState<IEnumerable<string>>{
            Value = selectedItems,
            IsValid = true
        };
    }
}