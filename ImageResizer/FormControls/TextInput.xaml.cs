using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

/// <summary>
///   A text input with an error message and, optionally, a label. Implements the following features:
///   <list type="bullet">
///   <item>Error messages are hidden by default.</item>
///   <item>When the user enters a value, if that value is invalid, error messages and styles are applied immediately.</item>
///   <item>
///     When the Revalidate method is called, the validator is executed and error styles are applied if the result is
///     invalid and the input has previously received user input, otherwise these styles are cleared.
///   </item>
///   <item>
///     When the DisplayErrors() method is called, error styles are applied immediately and the error message becomes
///     visible if it was previously hidden, and thereafter errors will be displayed when the field becomes invalid.
///   </item>
///   <item>
///     When the Reset() method is called, the value of the input is reset to the default value. This value is revalidated
///     to produce the new state of the input and errors are hidden.
///   </item>
/// </list>
/// </summary>
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
    private bool _isResetting = false;
    
    public TextInput
    (
        string? labelText,
        string defaultValue,
        Func<string, IValidatorResult> validate,
        int maxLength,
        AcceptedCharacters accepts
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

        if (labelText != null)
        {
            CreateLabel(labelText);
        }
        
        CreateEntryElement(maxLength);
        InitializeState();
    }
    
    public void Revalidate()
    {
        var value = _entryElement.Text;
        var validatorResult = _validate(value);
        State = new FormElementState<string>()
        {
            Value = value,
            IsValid = validatorResult.IsValid,
            ErrorMessage = validatorResult.ErrorMessage,
        };
    }

    public void DisplayErrors()
    {
        ShouldDisplayErrors = true;
        ApplyValidityDependentStyles();
    }

    public void Reset()
    {
        _isResetting = true;
        ShouldDisplayErrors = false;
        _entryElement.Text = _defaultValue;
        _isResetting = false;
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
        
        _entryElement.TextChanged += (sender, e) =>
        {
            /*
               Filtering input should be handled by native Entry handlers. This code is preserved as a fallback, but it 
               should not execute.
            */
            if (
                (Accepts == AcceptedCharacters.WholeNumbers || Accepts == AcceptedCharacters.PositiveIntegers) && 
                !FormControlHelpers.IsIntegerOrEmptyString(e.NewTextValue, Accepts == AcceptedCharacters.WholeNumbers)
            )
            {
                ((Entry)sender).Text = FormControlHelpers.ToIntegerOrEmptyString(e.NewTextValue, Accepts == AcceptedCharacters.WholeNumbers);
                return;
            }

            if (!_isResetting)
            {
                ShouldDisplayErrors = true;
            }

            var validatorResult = _validate(e.NewTextValue);
            
            State = new FormElementState<string>
            {
                Value = e.NewTextValue,
                IsValid = validatorResult.IsValid,
                ErrorMessage = validatorResult.ErrorMessage,
            };
        };
        
        _entryElement.Completed += (sender, e) => 
            Completed?.Invoke(this, e);
        
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

