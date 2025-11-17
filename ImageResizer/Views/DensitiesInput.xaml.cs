using System.ComponentModel;
using ImageResizer.Models;
using ImageResizer.ViewModels;
using Microsoft.Maui.Layouts;

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
        SetWidth();
        InitializeBaseWidthInput();
        InitializeCheckboxGroup();
    }
    
    public void RevealErrors()
    {
        _baseWidthInput.RevealErrors();
    }

    private void SetWidth()
    {
        MainLayout.MinimumWidthRequest =  AppDimensions.CONTENT_WIDTH;
        MainLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
    }

    private void InitializeBaseWidthInput()
    {
        _baseWidthInput = new TextInput(
            "",
            FormElementHelpers.CreateMinMaxValidator
            (
                _minBaseWidth, 
                _maxBaseWidth, 
                $"Please enter a valid base width (min. {_minBaseWidth}, max. {_maxBaseWidth})."
            ),
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Base Width",
            MaxLength = _maxBaseWidth.ToString().Length,
            MinimumWidthRequest = AppDimensions.CONTENT_WIDTH,
            MaximumWidthRequest = AppDimensions.CONTENT_WIDTH
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
        
        MainLayout.Children.Add(_baseWidthInput);
    }

    private void InitializeCheckboxGroup()
    {
        var checkboxGroup = new CheckboxGroup(
            [
                new CheckboxGroupItem
                {
                    Value = Density.OneX.ToHtmlString(), 
                    Label = Density.OneX.ToHtmlString(),
                    IsChecked = true, 
                    IsFrozen = true
                },
                new CheckboxGroupItem
                {
                    Value = Density.OneDot5X.ToHtmlString(), 
                    Label = Density.OneDot5X.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.TwoX.ToHtmlString(), 
                    Label = Density.TwoX.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.ThreeX.ToHtmlString(), 
                    Label = Density.ThreeX.ToHtmlString(),
                    IsChecked = true, 
                },
                new CheckboxGroupItem
                {
                    Value = Density.FourX.ToHtmlString(), 
                    Label = Density.FourX.ToHtmlString(),
                    IsChecked = true, 
                },
            ]
        )
        {
            LabelText = "Densities",
        };

        checkboxGroup.StateChanged += (sender, e) =>
        {
            IEnumerable<Density> selectedDensities = e.State.Value.Select(density =>
            {
                return Enum.GetValues<Density>().First(d => d.ToHtmlString() == density);
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
        
        MainLayout.Children.Add(checkboxGroup);
    }
}