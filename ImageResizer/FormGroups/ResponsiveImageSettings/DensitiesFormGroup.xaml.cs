using ImageResizer.DataModel;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class DensitiesFormGroup : ContentView
{
    private TextInput _baseWidthInput;
    private CustomCheckboxGroup _selectedDensities;
    private (int Min, int Max) _baseWidth = (1, 10_000);
    
    public DensitiesFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    private void InitializeFormControls()
    {
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
        
        RootLayout.Children.Add(_selectedDensities);
    }
}