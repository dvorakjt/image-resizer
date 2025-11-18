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
            StateChanged?.Invoke(this, field);
            SetErrorMessageText();

            if (IsPristine)
            {
                IsPristine = field.Value == _defaultValue;
                return;
            }

            if (!value.IsValid)
            {
                ApplyErrorStyles();
            }
            else
            {
                ClearErrorStyles();
            }
        }
    }

    public event EventHandler<IFormElementState<string>>? StateChanged;
    public new event PropertyChangedEventHandler? PropertyChanged;

    private bool IsPristine
    {
        get;
        set
        {
            if (value) ClearErrorStyles();
            field = value;
        }
    } = true;

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
            CreateLabelAndSetRootElementHeight(labelText);
        }
        
        CreateEntryElement(maxLength);
    }
    
    public void Revalidate()
    {
        var newValue = _entryElement.Text;
        var validatorResult = _validate(newValue);

        var newState = new FormElementState<string>()
        {
            Value = newValue,
            IsValid = validatorResult.IsValid,
            ErrorMessage = validatorResult.ErrorMessage,
        };

        State =  newState;
    }

    public void DisplayErrors()
    {
        IsPristine = false;
        if (!State.IsValid)
        {
            ApplyErrorStyles();
        }
    }

    public void Reset()
    {
        IsPristine = true;
        _entryElement.Text = _defaultValue;
    }

    private void CreateLabelAndSetRootElementHeight(string labelText)
    {
        RootLayout.HeightRequest = 63;
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

            Revalidate();
        };
        
        Border.Content = _entryElement;
        
        // Calling revalidate to set the initial state.
        Revalidate();
    }
    
    private void SetErrorMessageText()
    {
        ErrorMessage.Text = State.ErrorMessage;
    }

    private void ApplyErrorStyles()
    {
        Border.Stroke = Color.Parse("Red");
        ErrorMessage.IsVisible = true;
    }

    private void ClearErrorStyles()
    {
        Border.Stroke = Color.Parse("Black");
        ErrorMessage.IsVisible = false;
    }
}