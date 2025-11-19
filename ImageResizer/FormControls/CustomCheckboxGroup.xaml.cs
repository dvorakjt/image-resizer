using System.ComponentModel;
using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

public class CheckboxGroupItem
{
    public required string Label { get; init; }
    public required string Value { get; init; }
    public bool IsChecked { get; init; }
    public bool IsFrozen { get; init; }
}

public partial class CustomCheckboxGroup : ContentView, IFormElement<IEnumerable<string>>, INotifyPropertyChanged
{
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
    
    public event EventHandler<IFormElementState<IEnumerable<string>>>? StateChanged;
    public new event EventHandler<PropertyChangedEventArgs> PropertyChanged;

    public IFormElementState<IEnumerable<string>> State
    {
        get;
        set
        {
            field = value;
            StateChanged?.Invoke(this, value);
        }
    }

    private IEnumerable<CheckboxGroupItem> _items;
    
    public CustomCheckboxGroup(IEnumerable<CheckboxGroupItem> items)
    {
        if (!items.Any())
        {
            throw new ArgumentException("Items cannot be empty.");
        }
        InitializeComponent();
        
        _items = items;
        InitializeInputElements();
    }

    private void InitializeInputElements()
    {
        IList<string> selectedItems = new List<string>();
        
        foreach (var item in _items)
        {
            var checkbox = new CustomCheckbox()
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
                    IsValid = true,
                    ErrorMessage = ""
                };
            };
            
            CheckboxesLayout.Children.Add(checkbox);
        }

        State = new FormElementState<IEnumerable<string>>{
            Value = selectedItems,
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
        for (int i = 0; i < _items.Count(); i++)
        {
            var item = _items.ElementAt(i);
            var checkbox = (CustomCheckbox)CheckboxesLayout.Children[i];
            checkbox.IsChecked = item.IsChecked;
            checkbox.IsEnabled = !item.IsFrozen;
        }
    }
}