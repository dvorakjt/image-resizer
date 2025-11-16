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
    private readonly Func<string, string> _filterInput;
    private string _defaultValue;
    
    public TextInput(string defaultValue, Func<string, ValidatorFuncResult> validationFunc, Func<string, string>? filterInput = null)
    {
        InitializeComponent();
        
        Console.WriteLine("Beginning constructor.");
        
        _defaultValue = defaultValue;
        _validationFunc = validationFunc;
        
        Console.WriteLine("Constructor 2.");
        
        if(filterInput == null)
        {
            filterInput = val => val;
        }
        
        Console.WriteLine("Constructor 3.");
        
        _filterInput = filterInput;
        
        var filteredInput = _filterInput(defaultValue);
        
        Console.WriteLine("Constructor 4.");
        
        var validationResult = _validationFunc(filteredInput);
        
        Console.WriteLine("Constructor 5.");

        State = new FormElementState<string>(
            defaultValue,
            validationResult.IsValid,
            validationResult.Message
        );
        
        Console.WriteLine("Ending constructor.");
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