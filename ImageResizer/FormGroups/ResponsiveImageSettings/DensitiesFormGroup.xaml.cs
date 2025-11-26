using ImageResizer.DataModel;
using ImageResizer.DataModel.ResponsiveImageSettings;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class DensitiesFormGroup : ContentView, IFormElement<DensitiesFormGroupValue>
{
    public event EventHandler<IFormElementState<DensitiesFormGroupValue>>? StateChanged;

    public IFormElementState<DensitiesFormGroupValue> State
    {
        get
        {
            var isValid =
                _baseWidthInput.State.IsValid && _selectedDensities.State.IsValid && _defaultWidthInput.State.IsValid;
            
            int? baseImageWidth, defaultImageWidth;
            baseImageWidth = defaultImageWidth = null;

            if (int.TryParse(_baseWidthInput.State.Value, out int b))
            {
                baseImageWidth = b;
            }

            if (int.TryParse(_defaultWidthInput.State.Value, out int d))
            {
                defaultImageWidth = d;
            }

            var densities = _selectedDensities.State.Value.Select(d =>
            {
                foreach (var density in Enum.GetValues(typeof(Density)))
                {
                    if(d == density.ToString()) return (Density)density;
                }

                throw new InvalidOperationException($"Unsupported density: {d}");
            });

            return new FormElementState<DensitiesFormGroupValue>
            {
                Value = new DensitiesFormGroupValue
                {
                    Densities = densities,
                    BaseImageWidth = baseImageWidth,
                    DefaultImageWidth = defaultImageWidth,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
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
    
    public void DisplayErrors()
    {
        _baseWidthInput.DisplayErrors();
        _selectedDensities.DisplayErrors();
        _defaultWidthInput.DisplayErrors();
    }

    public void Revalidate()
    {
        _baseWidthInput.Revalidate();
        _selectedDensities.Revalidate();
        _defaultWidthInput.Revalidate();
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

        _selectedDensities.StateChanged += 
            (sender, e) => StateChanged?.Invoke(this, State);
        
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
        
        _baseWidthInput.StateChanged += 
            (sender, e) => StateChanged?.Invoke(this, State);
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

        _defaultWidthInput.StateChanged += 
            (sender, e) => StateChanged?.Invoke(this, State);
        RootLayout.Children.Add(_defaultWidthInput);
    }
}