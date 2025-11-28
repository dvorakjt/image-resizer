using ImageResizer.DataModel;
using ImageResizer.DataModel.Formats;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Formats;

public partial class JPEGOptionsFormGroup : ContentView, IFormElement<JPEGOptionsFormGroupValue>
{
    public event EventHandler<IFormElementState<JPEGOptionsFormGroupValue>>? StateChanged;

    public IFormElementState<JPEGOptionsFormGroupValue> State
    {
        get
        {
            int? quality = null;

            if (int.TryParse(_qualityInput.State.Value, out int q))
            {
                quality = q;
            }

            var isValid = _qualityInput.State.IsValid;

            return new FormElementState<JPEGOptionsFormGroupValue>()
            {
                Value = new JPEGOptionsFormGroupValue()
                {
                    Quality = quality,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
    private TextInput _qualityInput;
    private int _defaultQuality = 90;
    private (int Min, int Max) _quality = (0, 100);
    
    public JPEGOptionsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }
    
    public void DisplayErrors()
    {
        _qualityInput.DisplayErrors();
    }

    public void Revalidate()
    {
        _qualityInput.Revalidate();
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
        _qualityInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        formControlsLayout.Children.Add(_qualityInput);
        RootLayout.Children.Add(formControlsLayout);
    }
}