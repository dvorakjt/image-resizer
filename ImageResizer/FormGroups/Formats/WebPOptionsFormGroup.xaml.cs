using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Formats;

public partial class WebPOptionsFormGroup : ContentView
{
    private TextInput _qualityInput;
    private TextInput _effortInput;
    private int _defaultQuality = 75;
    private int _defaultEffort = 4;
    private (int Min, int Max) _quality = (0, 100);
    private (int Min, int Max) _effort = (0, 6);
    
    public WebPOptionsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    public void InitializeFormControls()
    {
        var spacing = 2;
        var inputWidth = (AppDimensions.ColumnWidth - spacing) / 2;
        var formControlsLayout = new HorizontalStackLayout()
        {
            Spacing = spacing
        };
        
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
        
        _qualityInput.WidthRequest = inputWidth;
        formControlsLayout.Children.Add(_qualityInput);
        
        _effortInput = new TextInputBuilder()
            .WithLabel("Effort")
            .WholeNumbersOnly()
            .WithDefaultValue(_defaultEffort.ToString())
            .WithValidator
            (
                FormControlHelpers.CreateMinMaxValidator
                (
                    _effort.Min, 
                    _effort.Max, 
                    $"Please enter an integer between {_effort.Min} and {_effort.Max}"
                )
            )
            .WithMaxLength(_effort.Max.ToString().Length)
            .Build();
        
        _effortInput.WidthRequest = inputWidth;
        formControlsLayout.Children.Add(_effortInput);
        
        RootLayout.Children.Add(formControlsLayout);
    }
}