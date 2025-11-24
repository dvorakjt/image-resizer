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
            ErrorMessage.Text = field.ErrorMessage;
            ApplyValidityDependentStyles();
            StateChanged?.Invoke(this, field);
        }
    }

    // Flag that enables the restriction of input to certain character sets
    public AcceptedCharacters Accepts { get; private set; }

    public event EventHandler<IFormElementState<string>>? StateChanged;
    public event EventHandler? Completed;
    
    public bool ShouldDisplayErrors
    {
        get;
        private set
        {
            field = value;
            ErrorMessage.IsVisible = field;
        }
    }

    private Entry _entryElement;
    private readonly string _defaultValue;
    private readonly Func<string, IValidatorResult> _validate;
    private readonly bool _displayErrorsOnInput;
    
    public TextInput
    (
        string? labelText,
        string defaultValue,
        Func<string, IValidatorResult> validate,
        int maxLength,
        AcceptedCharacters accepts,
        bool displayErrorsOnInput
    )
    {
        InitializeComponent();

        if (
            (accepts == AcceptedCharacters.WholeNumbers || accepts == AcceptedCharacters.PositiveIntegers) && 
            !FormControlHelpers.IsIntegerOrEmptyString(defaultValue, accepts == AcceptedCharacters.WholeNumbers))
        {
            throw new ArgumentException(
                "A numeric input must have a numeric default or an empty string."
            );
        }

        Accepts = accepts;
        _defaultValue = defaultValue;
        _validate = validate;
        _displayErrorsOnInput = displayErrorsOnInput;

        if (labelText != null)
        {
            CreateLabel(labelText);
        }
        
        CreateEntryElement(maxLength);
        ValidateEnteredValueAndUpdateState();
    }
    
    public void Revalidate()
    {
        ValidateEnteredValueAndUpdateState();
    }

    public void DisplayErrors()
    {
        ShouldDisplayErrors = true;
        ApplyValidityDependentStyles();
    }

    public void Reset()
    {
        /*
            Here, ValidateEnteredValueAndUpdateState is invoked explicitly so that even if the input was already
            set to the default value, state is still updated and the appropriate styles are applied. The event 
            listener is first removed so that ShouldDisplayErrors is not set to true. After state and the value of the 
            entry element are reset, the OnTextChanged event handler is reattached to the TextChanged event.
        */
        ShouldDisplayErrors = false;
        _entryElement.TextChanged -= OnTextChanged;
        _entryElement.Text = _defaultValue;
        ValidateEnteredValueAndUpdateState();
        _entryElement.TextChanged += OnTextChanged;
    }

    public new void Focus()
    {
        _entryElement.Focus();
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
        
        _entryElement.TextChanged += OnTextChanged;
        _entryElement.Completed += (sender, e) => 
            Completed?.Invoke(this, e);

        if (_displayErrorsOnInput)
        {
            _entryElement.Unfocused += (sender, e) => DisplayErrors();
        }
        
        Border.Content = _entryElement;
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        // On Windows, this is handled by a native event handler, but on Mac, it must be handled this way so that the 
        // MaxLength property of the Entry element is honored.
        if (
            (Accepts == AcceptedCharacters.WholeNumbers || Accepts == AcceptedCharacters.PositiveIntegers) && 
            !FormControlHelpers.IsIntegerOrEmptyString(e.NewTextValue, Accepts == AcceptedCharacters.WholeNumbers)
        )
        {
            ((Entry)sender).Text = FormControlHelpers.ToIntegerOrEmptyString(e.NewTextValue, Accepts == AcceptedCharacters.WholeNumbers);
            return;
        }

        if (_displayErrorsOnInput)
        {
            ShouldDisplayErrors = true;
        }
        else
        {
            ShouldDisplayErrors = false;
        }

        ValidateEnteredValueAndUpdateState();
    }

    private void ValidateEnteredValueAndUpdateState()
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

    private void ApplyValidityDependentStyles()
    {
        if (ShouldDisplayErrors && !State.IsValid)
        {
            Border.Stroke = Color.Parse("Red");
        }
        else
        {
            Border.Stroke = Color.Parse("Black");
        }
    }
}

