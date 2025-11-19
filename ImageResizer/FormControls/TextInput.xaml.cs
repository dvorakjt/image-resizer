using System.ComponentModel;
using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

public partial class TextInput : ContentView, IFormElement<string>
{
    public IFormElementState<string> State
    {
        get;
        private set
        {
            field = value;
            
            /*
                Always update the text of the error message field so that it appears when revealed either due to user 
                interaction or because the DisplayErrors() method is called from outside the component.
            */ 
            ErrorMessage.Text = field.ErrorMessage;
            StateChanged?.Invoke(this, field);
        }
    }

    public event EventHandler<IFormElementState<string>>? StateChanged;
    public new event PropertyChangedEventHandler? PropertyChanged;

    private Entry _entryElement;
    private readonly string _defaultValue;
    private readonly Func<string, IValidatorResult> _validate;
    private readonly bool _isNumeric;
    private readonly bool _allowZero;
    
    public TextInput
    (
        string? labelText,
        string defaultValue,
        Func<string, IValidatorResult> validate,
        int maxLength,
        bool isNumeric,
        bool allowZero
    )
    {
        InitializeComponent();

        if (isNumeric && !FormControlHelpers.IsIntegerOrEmptyString(defaultValue, allowZero))
        {
            throw new ArgumentException(
                "A numeric input must have a numeric default or an empty string."
            );
        }

        _defaultValue = defaultValue;
        _validate = validate;
        _isNumeric = isNumeric;
        _allowZero = allowZero;

        if (labelText != null)
        {
            CreateLabel(labelText);
        }
        
        CreateEntryElement(maxLength);
        InitializeState();
    }
    
    public void Revalidate()
    {
        InitializeState();
    }

    public void DisplayErrors()
    {
        if (!State.IsValid)
        {
            Border.Stroke = Color.Parse("Red");
            ErrorMessage.IsVisible = true;
        }
    }

    public void Reset()
    {
        _entryElement.Text = _defaultValue;
        HideErrors();
    }

    private void CreateLabel(string labelText)
    {
        var label = new Label
        {
            Text = labelText,
        };
        
        RootLayout.Children.Insert(0, label);
    }

    private void CreateEntryElement(int maxLength)
    {
        _entryElement = new Entry()
        {
            Text = _defaultValue,
            MaxLength = maxLength,
            HorizontalOptions = LayoutOptions.Fill
        };
        
        _entryElement.TextChanged += (sender, e) =>
        {
            if (_isNumeric && !FormControlHelpers.IsIntegerOrEmptyString(e.NewTextValue, _allowZero))
            {
                ((Entry)sender).Text = FormControlHelpers.ToIntegerOrEmptyString(e.NewTextValue, _allowZero);
                return;
            }

            var validatorResult = _validate(e.NewTextValue);
            
            State = new FormElementState<string>
            {
                Value = e.NewTextValue,
                IsValid = validatorResult.IsValid,
                ErrorMessage = validatorResult.ErrorMessage,
            };

            if (State.IsValid)
            {
                HideErrors();
            }
            else
            {
                DisplayErrors();
            }
        };
        
        Border.Content = _entryElement;
    }

    private void InitializeState()
    {
        var value = _entryElement.Text;
        var validatorResult = _validate(value);
        State = new FormElementState<string>
        {
            Value = value,
            IsValid = validatorResult.IsValid,
            ErrorMessage = validatorResult.ErrorMessage,
        };
    }
    
    private void HideErrors()
    {
        Border.Stroke = Color.Parse("Black");
        ErrorMessage.IsVisible = false;
    }
}