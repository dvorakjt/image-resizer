using System.ComponentModel;

namespace ImageResizer.Components;

public partial class TextInput : ContentView, IFormElement<string>, ISettableFormElement<string>, IFormElementWithErrorDisplay, INotifyPropertyChanged
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
            field = value;
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<string>(field));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorMessageText)));
        }
    }

    public string Value { get => State.Value; }
    public string ErrorMessageText { get => State.ErrorMessage ?? ""; }
    public bool IsErrorMessageVisible
    {
        get;
        private set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsErrorMessageVisible)));
        }
    }

    private readonly Func<string, ValidatorFuncResult> _validationFunc;
    private bool _isPristine = true;
    
    public TextInput(string defaultValue, Func<string, ValidatorFuncResult> validationFunc)
    {
        InitializeComponent();    
        _validationFunc = validationFunc;
        
        var validationResult = _validationFunc(defaultValue);

        State = new FormElementState<string>(
            defaultValue,
            validationResult.IsValid,
            validationResult.Message
        );
    }
    
    public void SetValue(string value)
    {
        var validationResult = _validationFunc(value);

        State = new FormElementState<string>(
            value,
            validationResult.IsValid,
            validationResult.Message
        );
    }

    public void RevealErrors()
    {
        IsErrorMessageVisible = true;
    }
    
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        SetValue(e.NewTextValue);
        if (_isPristine)
        {
            _isPristine = false;
        }
        else
        {
            RevealErrors();
        }
    }
}