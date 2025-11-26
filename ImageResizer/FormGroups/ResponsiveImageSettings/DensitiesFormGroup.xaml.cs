using ImageResizer.DataModel;
using ImageResizer.DataModel.ResponsiveImageSettings;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class DensitiesFormGroup : ContentView
{
    private TextInput _baseWidthInput;
    private CustomCheckboxGroup _selectedDensities;
    private TextInput _defaultWidthInput;
    private (int Min, int Max) _baseWidth = (1, 10_000);
    private (int Min, int Max) _defaultWidth = (1, 40_000);
    
    public DensitiesFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    public void Reset()
    {
        _baseWidthInput.Reset();
        _selectedDensities.Reset();
        _defaultWidthInput.Reset();
    }

    private void InitializeFormControls()
    {
        _selectedDensities = new CustomCheckboxGroup(
            [
                new CheckboxGroupItem
                {
                    Value = Density.OneX.ToString(),
                    Label = Density.OneX.ToHtmlString(),
                    IsChecked = true, 
                    IsFrozen = true
                },
                new CheckboxGroupItem
                {
                    Value = Density.OneDot5X.ToString(),
                    Label = Density.OneDot5X.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.TwoX.ToString(),
                    Label = Density.TwoX.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.ThreeX.ToString(),
                    Label = Density.ThreeX.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.FourX.ToString(),
                    Label = Density.FourX.ToHtmlString(),
                    IsChecked = true, 
                },
            ]
        )
        {
            LabelText = "Densities",
        };
        
        // Add a margin to even out spacing since densities does not contain an error message
        _selectedDensities.Margin = new Thickness(0, 0, 0, 13);
        RootLayout.Children.Add(_selectedDensities);
        
        _baseWidthInput = new TextInputBuilder()
            .WithLabel("Base Width")
            .PositiveIntegersOnly()
            .WithValidator
            (
                FormControlHelpers.CreateMinMaxValidator
                (
                    _baseWidth.Min,
                    _baseWidth.Max,
                    $"Please enter a number between {_baseWidth.Min} and {_baseWidth.Max}"
                )
            )
            .WithMaxLength(_baseWidth.Max.ToString().Length)
            .Build();
        
        _baseWidthInput.HorizontalOptions = LayoutOptions.Fill;
        RootLayout.Children.Add(_baseWidthInput);

        _defaultWidthInput = new TextInputBuilder()
            .WithLabel("Default Image Width")
            .PositiveIntegersOnly()
            .WithValidator(FormControlHelpers.CreateMinMaxValidator(
                _defaultWidth.Min,
                _defaultWidth.Max,
                $"Please enter a number between {_defaultWidth.Min} and {_defaultWidth.Max}")
            )
            .WithMaxLength(_defaultWidth.Max.ToString().Length)
            .Build();

        
        RootLayout.Children.Add(_defaultWidthInput);
    }
}