using System.ComponentModel;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public partial class TextInput : ContentView, IFormElement<string>, IResettableFormElement, IFormElementWithErrorDisplay, INotifyPropertyChanged
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
    
    public static BindableProperty MaxLengthProperty =
        BindableProperty.Create("MaxLength", typeof(int), typeof(TextInput), int.MaxValue);

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set {
            SetValue(MaxLengthProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxLength)));
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderColor)));
        }
    }

    public string Value { get => State.Value; }
    public string ErrorMessageText { get => State.ErrorMessage ?? ""; }

    public bool IsErrorMessageVisible
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsErrorMessageVisible)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderColor)));
        }
    } = false;

    public Color BorderColor
    {
        get => !State.IsValid && IsErrorMessageVisible ? Color.Parse("Red") : Color.Parse("Black");
    }

    private readonly Func<string, ValidatorFuncResult> _validationFunc;
    private readonly Func<string, string> _filterInput;
    private string _defaultValue;
    
    public TextInput(string defaultValue, Func<string, ValidatorFuncResult> validationFunc, Func<string, string>? filterInput = null)
    {
        InitializeComponent();
        
        _defaultValue = defaultValue;
        _validationFunc = validationFunc;
        
        if(filterInput == null)
        {
            filterInput = val => val;
        }
        
        _filterInput = filterInput;
        
        var filteredInput = _filterInput(defaultValue);
        
        var validationResult = _validationFunc(filteredInput);

        State = new FormElementState<string>(
            defaultValue,
            validationResult.IsValid,
            validationResult.Message
        );
    }

    public void Reset()
    {
        IsErrorMessageVisible = false;
        var filteredValue = _filterInput(_defaultValue);
        var validationResult = _validationFunc(filteredValue);

        State = new FormElementState<string>(
            filteredValue,
            validationResult.IsValid,
            validationResult.Message
        );
    }

    public void RevealErrors()
    {
        IsErrorMessageVisible = true;
    }

    public void Revalidate()
    {
        var validationResult = _validationFunc(State.Value);

        State = new FormElementState<string>()
        {
            Value = State.Value,
            IsValid = validationResult.IsValid,
            ErrorMessage = validationResult.Message
        };
    }
    
    
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var filteredValue = _filterInput(e.NewTextValue);
        var changed = State.Value != filteredValue;
        var validationResult = _validationFunc(filteredValue);

        State = new FormElementState<string>(
            filteredValue,
            validationResult.IsValid,
            validationResult.Message
        );

        if (changed)
        {
            RevealErrors();
        }
    }
}