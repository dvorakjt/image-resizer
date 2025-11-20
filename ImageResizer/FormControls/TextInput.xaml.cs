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

    public AcceptedCharacters Accepts { get; private set; }

    public event EventHandler<IFormElementState<string>>? StateChanged;
    public event EventHandler? Completed;

    private Entry _entryElement;
    private readonly string _defaultValue;
    private readonly Func<string, IValidatorResult> _validate;
    
    
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
#if MACCATALYST
            /* 
                On Windows, this will be handled by the native TextChanging event because when handled in this manner, the 
                unfiltered value of the entry element briefly appears. On Mac, this is not an issue, so it can be handled 
                more simply.
            */
            if (
                (Accepts == AcceptedCharacters.WholeNumbers || Accepts == AcceptedCharacters.PositiveIntegers) && 
                !FormControlHelpers.IsIntegerOrEmptyString(e.NewTextValue, Accepts == AcceptedCharacters.WholeNumbers)
            )
            {
                ((Entry)sender).Text = FormControlHelpers.ToIntegerOrEmptyString(e.NewTextValue, Accepts == AcceptedCharacters.WholeNumbers);
                return;
            }
#endif
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
        
        _entryElement.Completed += (sender, e) => Completed?.Invoke(this, e);
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