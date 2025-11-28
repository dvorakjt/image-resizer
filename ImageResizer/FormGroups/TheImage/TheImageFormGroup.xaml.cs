using ImageResizer.DataModel;
using ImageResizer.DataModel.TheImage;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.TheImage;

public partial class TheImageFormGroup : ContentView, IFormElement<TheImageFormGroupValue>
{
    
    public event EventHandler<IFormElementState<TheImageFormGroupValue>>? StateChanged;

    public IFormElementState<TheImageFormGroupValue> State
    {
        get
        {
            var isValid = _imagePicker.State.IsValid && _altTextInput.State.IsValid;
            return new FormElementState<TheImageFormGroupValue>()
            {
                Value = new TheImageFormGroupValue()
                {
                    ImageStream = _imagePicker.State.Value,
                    AltText = _altTextInput.State.Value,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }

    private ImagePicker _imagePicker;
    private TextInput _altTextInput;
    
    public TheImageFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    private void InitializeFormControls()
    {
        _imagePicker = new ImagePicker();
        _imagePicker.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        RootLayout.Children.Add(_imagePicker);

        _altTextInput = new TextInputBuilder()
            .WithLabel("Alt Text")
            .WithValidator(
                FormControlHelpers.CreateRequiredFieldValidator("Please enter a description of the selected image.")
            )
            .Build();
        
        _altTextInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        RootLayout.Children.Add(_altTextInput);
    }
    
    public void DisplayErrors()
    {
        _imagePicker.DisplayErrors();
        _altTextInput.DisplayErrors();
    }

    public void Revalidate()
    {
        _imagePicker.Revalidate();
        _altTextInput.Revalidate();
    }

    public void Reset()
    {
        _imagePicker.Reset();
        _altTextInput.Reset();
    }
}