namespace ImageResizer.Components;

public partial class TextInput : ContentView, IFormElement<string>
{
    public static BindableProperty LabelTextProperty = BindableProperty.Create("LabelText", typeof(string), typeof(TextInput), default(string));

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }
    
    public event EventHandler<FormElementEventArgs<string>> StateChanged;
    
    private Func<string, ValidationResult> _validationFunc;
    
    public TextInput(Func<string, ValidationResult> validationFunc)
    {
        InitializeComponent();
        _validationFunc = validationFunc;
        BindingContext = this;
    }

    private void OnValueChanged(object sender, TextChangedEventArgs e)
    {
        var validationResult = _validationFunc(e.NewTextValue);
        
        ValidationMessage.Text =  validationResult.Message;
        var stateChangedEventArgs = new FormElementEventArgs<string>(
            e.NewTextValue,
            validationResult.Validity
        );
        
        StateChanged?.Invoke(this, stateChangedEventArgs);
    }
}