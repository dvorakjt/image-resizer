using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Formats;

public partial class JPEGOptionsFormGroup : ContentView
{
    private TextInput _qualityInput;
    private int _defaultQuality = 90;
    private (int Min, int Max) _quality = (0, 100);
    
    public JPEGOptionsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    public void Reset()
    {
        _qualityInput.Reset();
    }

    private void InitializeFormControls()
    {
        var formControlsLayout = new HorizontalStackLayout();
        
        _qualityInput = new TextInputBuilder()
            .WithLabel("Quality")
            .WholeNumbersOnly()
            .WithDefaultValue(_defaultQuality.ToString())
            .WithValidator
            (
                FormControlHelpers.CreateMinMaxValidator
                (
                    _quality.Min, 
                    _quality.Max, 
                    $"Please enter an integer between {_quality.Min} and {_quality.Max}"
                )
            )
            .WithMaxLength(_quality.Max.ToString().Length)
            .Build();

        _qualityInput.WidthRequest = AppDimensions.ColumnWidth;
        formControlsLayout.Children.Add(_qualityInput);
        
        RootLayout.Children.Add(formControlsLayout);
    }
}