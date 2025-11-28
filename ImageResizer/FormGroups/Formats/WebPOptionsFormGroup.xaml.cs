using ImageResizer.DataModel;
using ImageResizer.DataModel.Formats;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Formats;

public partial class WebPOptionsFormGroup : ContentView, IFormElement<WebPOptionsFormGroupValue>
{
    public event EventHandler<IFormElementState<WebPOptionsFormGroupValue>>? StateChanged;
    public IFormElementState<WebPOptionsFormGroupValue> State
    {
        get
        {
            int? quality, effort;
            quality = effort = null;

            if (int.TryParse(_qualityInput.State.Value, out int q))
            {
                quality = q;
            }
            
            if (int.TryParse(_effortInput.State.Value, out int e))
            {
                effort = e;
            }
            
            var isValid = _qualityInput.State.IsValid && _effortInput.State.IsValid;

            return new FormElementState<WebPOptionsFormGroupValue>()
            {
                Value = new WebPOptionsFormGroupValue()
                {
                    Quality = quality,
                    Effort = effort
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }

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
    
    public void DisplayErrors()
    {
        _qualityInput.DisplayErrors();
        _effortInput.DisplayErrors();
    }
    
    public void Revalidate()
    {
        _qualityInput.Revalidate();
        _effortInput.Revalidate();
    }
    
    public void Reset()
    {
        _qualityInput.Reset();
        _effortInput.Reset();
    }

    private void InitializeFormControls()
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

        _qualityInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
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
        
        _effortInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        _effortInput.WidthRequest = inputWidth;
        formControlsLayout.Children.Add(_effortInput);
        
        RootLayout.Children.Add(formControlsLayout);
    }
}