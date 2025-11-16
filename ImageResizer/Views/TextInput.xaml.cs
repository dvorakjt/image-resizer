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
    private bool _isPristine = true;
    
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
           
        var validationResult = _validationFunc(_filterInput(defaultValue));

        State = new FormElementState<string>(
            defaultValue,
            validationResult.IsValid,
            validationResult.Message
        );
    }

    public void Reset()
    {
        IsErrorMessageVisible = false;
        _isPristine = true;
        SetValue(_defaultValue);
    }

    public void RevealErrors()
    {
        IsErrorMessageVisible = true;
    }
    
    private void SetValue(string value)
    {
        var filteredValue = _filterInput(value);
        var validationResult = _validationFunc(filteredValue);

        State = new FormElementState<string>(
            filteredValue,
            validationResult.IsValid,
            validationResult.Message
        );
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