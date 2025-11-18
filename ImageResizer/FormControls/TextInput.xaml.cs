using System.ComponentModel;
using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

/// <summary>
/// A simple text input that can also display error messages and a label. Can accept a validation function and can be
/// configured to modify input before setting the value of the underlying entry element. 
/// </summary>
public partial class TextInput : ContentView, IFormElement<string>
{
    public static TextInput Create
    (
        string defaultValue, 
        Func<string, ValidatorResult> validate, 
        int maxLength = int.MaxValue
    )
    {
        return new TextInput(defaultValue, validate, maxLength, false, true);
    }
    
    public static TextInput CreateNumeric
    (
        string defaultValue, 
        Func<string, ValidatorResult> validate, 
        int maxLength = int.MaxValue,
        bool allowZero = true
    )
    {
        return new TextInput(defaultValue, validate, maxLength, true, allowZero);
    }
    
    public static BindableProperty LabelTextProperty = BindableProperty.Create
        (
            nameof(LabelText), 
            typeof(string), 
            typeof(TextInput), 
            ""
        );

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set
        {
            SetValue(LabelTextProperty, value);
            OnPropertyChanged(nameof(LabelText));
            OnPropertyChanged(nameof(ShowLabel));
        }
    }

    public bool ShowLabel
    {
        get => !LabelText.IsWhiteSpace();
    }
    
    public IFormElementState<string> State
    {
        get
        {
            var value = _entryElement.Text;
            var validatorResult = _validate(value);

            var state = new FormElementState<string>()
            {
                Value = value,
                IsValid = validatorResult.IsValid,
                ErrorMessage = validatorResult.ErrorMessage,
            };

            return state;
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
        string defaultValue,
        Func<string, IValidatorResult> validate,
        int maxLength,
        bool isNumeric,
        bool allowZero
    )
    {
        InitializeComponent();

        _defaultValue = defaultValue;
        _validate = validate;
        _isNumeric = isNumeric;
        _allowZero = allowZero;

        CreateEntryElement(maxLength);
    }
    
    public void Revalidated()
    {
        StateChanged?.Invoke(this, State);
        SetErrorMessageText();

        if (State.IsValid)
        {
            ClearErrorStyles();
        } else if (!IsPristine)
        {
            ApplyErrorStyles();
        }
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

    private void CreateEntryElement(int maxLength)
    {
        _entryElement = new Entry()
        {
            Text = _defaultValue,
            MaxLength = maxLength,
            HorizontalOptions = LayoutOptions.Fill
        };
        
        var oldValue = _entryElement.Text;
        _entryElement.TextChanged += (sender, e) =>
        {
            if (_isNumeric && !FormControlHelpers.IsIntegerOrEmptyString(e.NewTextValue, _allowZero))
            {
                ((Entry)sender).Text = FormControlHelpers.ToIntegerOrEmptyString(e.NewTextValue, _allowZero);
                return;
            }
            
            StateChanged?.Invoke(this, State);
            SetErrorMessageText();
            var valueChanged = _entryElement.Text != oldValue;
            oldValue = _entryElement.Text;

            if (IsPristine)
            {
                IsPristine = !valueChanged;
                return;
            }
            
            if (State.IsValid)
            {
                ClearErrorStyles();
            }
            else
            {
                ApplyErrorStyles();
            }
        };
        
        Border.Content = _entryElement;
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

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}