using System.ComponentModel;
using ImageResizer.Models;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public struct DensitiesInputValue
{
    public int? BaseWidth { get; init; }
    public IEnumerable<Density> SelectedDensities { get; init; }
}

public partial class DensitiesInput : ContentView, IFormElement<DensitiesInputValue>, IFormElementWithErrorDisplay
{
    private static int _minBaseWidth = 1;
    private static int _maxBaseWidth = 10_000;
    
    public event EventHandler<FormElementStateChangedEventArgs<DensitiesInputValue>>? StateChanged;
    
    public FormElementState<DensitiesInputValue> State
    {
        get;
        private set
        {
            field = value;
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<DensitiesInputValue>(field));
        }
    }

    private TextInput _baseWidthInput;
    
    public DensitiesInput()
    {
        InitializeComponent();

        _baseWidthInput = new TextInput(
            "",
            (value) =>
            {
                var canParse = int.TryParse(value, out var width);
                bool isValid = canParse && width >= _minBaseWidth && width <= _maxBaseWidth;
 
                return new ValidatorFuncResult(
                    isValid,
                    isValid ? "" : $"Please enter a valid base width (min. {_minBaseWidth}, max. {_maxBaseWidth})."
                );
            },
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Base Width",
            MaxLength = _maxBaseWidth.ToString().Length,
        };

        _baseWidthInput.StateChanged += (sender, e) =>
        {
            var canParseInt = int.TryParse(e.State.Value, out var baseWidth);
            int? parsedBaseWidth = canParseInt ? baseWidth : null;

            State = new FormElementState<DensitiesInputValue>
            {
                Value = new DensitiesInputValue
                {
                    BaseWidth = parsedBaseWidth,
                    SelectedDensities = State.Value.SelectedDensities
                },
                IsValid = parsedBaseWidth != null
            };
        };

        var checkboxGroup = new CheckboxGroup(
            [
                new CheckboxGroupItem { Value = "1x", Label = "1x", IsChecked = true, IsFrozen = true },
                new CheckboxGroupItem { Value = "1.5x", Label = "1.5x", IsChecked = true },
                new CheckboxGroupItem { Value = "2x", Label = "2x", IsChecked = true },
                new CheckboxGroupItem { Value = "3x", Label = "3x", IsChecked = true },
                new CheckboxGroupItem { Value = "4x", Label = "4x", IsChecked = true },
            ]
        )
        {
            LabelText = "Densities"
        };

        checkboxGroup.StateChanged += (sender, e) =>
        {
            IEnumerable<Density> selectedDensities = e.State.Value.Select(d =>
            {
                switch (d)
                {
                    case "1x":
                        return Density.OneX;
                    case "1.5x":
                        return Density.OneDot5X;
                    case "2x":
                        return Density.TwoX;
                    case "3x":
                        return Density.ThreeX;
                    case "4x":
                        return Density.FourX;
                    default:
                        throw new InvalidEnumArgumentException("Density must be either 1x, 1.5x, 2x, 3x, or 4x");
                }
            });

            State = new FormElementState<DensitiesInputValue>
            {
                Value = new DensitiesInputValue()
                {
                    BaseWidth = State.Value.BaseWidth,
                    SelectedDensities = selectedDensities,
                },
                IsValid = State.IsValid
            };
        };
        
        State = new FormElementState<DensitiesInputValue>
        {
            Value = new DensitiesInputValue()
            {
                BaseWidth = null,
                SelectedDensities = [Density.OneX, Density.OneDot5X, Density.TwoX, Density.ThreeX, Density.FourX]
            },
            IsValid = State.IsValid
        };
        
        DensitiesInputLayout.Children.Add(_baseWidthInput);
        DensitiesInputLayout.Children.Add(checkboxGroup);
    }
    
    public void RevealErrors()
    {
        _baseWidthInput.RevealErrors();
    }
}