using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.TheImage;

public partial class TheImageFormGroup : ContentView
{
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
        RootLayout.Children.Add(_imagePicker);

        _altTextInput = new TextInputBuilder()
            .WithLabel("Alt Text")
            .WithValidator(
                FormControlHelpers.CreateRequiredFieldValidator("Please enter a description of the selected image.")
            )
            .Build();
        
        RootLayout.Children.Add(_altTextInput);
    }

    public void Reset()
    {
        _imagePicker.Reset();
        _altTextInput.Reset();
    }
}