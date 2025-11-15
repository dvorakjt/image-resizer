using System.ComponentModel;
using ImageResizer.Models;

namespace ImageResizer.Components;

public struct DensitiesInputValue
{
    public int? BaseWidth { get; init; }
    public IEnumerable<Density> SelectedDensities { get; init; }
}

public partial class DensitiesInput : ContentView, IFormElement<DensitiesInputValue>, IFormElementWithErrorDisplay
{
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
            FormElementHelpers.CreateRequiredFieldValidator("Please enter a base width."),
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Base Width"
        };

        _baseWidthInput.StateChanged += (sender, e) =>
        {
            int? parsedBaseWidth = e.State.Value.Length > 0 ? int.Parse(e.State.Value) : null;

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