using ImageResizer.DataModel;
using ImageResizer.FormControls;
using ImageResizer.Utils;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class WidthsFormGroup : ContentView
{
    private CustomRadioButtonGroup _widthOrderStrategyRadioGroup;
    private TextInput _newScreenWidthInput;
    private ISortedLiveList<ScreenAndImageWidths> _widths = new SortedLiveList<ScreenAndImageWidths>();
    private readonly int _minWidth = 1;
    private readonly int _maxWidth = 40_000;
    private readonly int _maxWidthCount = 30;
    
    public WidthsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    private void InitializeFormControls()
    {
       InitializeWidthOrderStrategyRadioGroup();
       InitializeNewScreenWidthInput();
    }

    private void InitializeWidthOrderStrategyRadioGroup()
    {
        var _widthOrderStrategyLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };

        var label = new Label()
        {
            Text = "Width Order Strategy",
        };
        
        _widthOrderStrategyLayout.Children.Add(label);

        _widthOrderStrategyRadioGroup = new CustomRadioButtonGroup
        (
            [
                new CustomRadioButtonGroupItem
                {
                    Content = "Max-Widths",
                    Value = WidthOrderStrategy.MaxWidths.ToString(),

                },
                new CustomRadioButtonGroupItem
                {
                    Content = "Min-Widths",
                    Value = WidthOrderStrategy.MinWidths.ToString(),
                }
            ],
            WidthOrderStrategy.MaxWidths.ToString(),
            "WidthOrderStrategy",
            18
        );
        
        _widthOrderStrategyLayout.Children.Add(_widthOrderStrategyRadioGroup);
        RootLayout.Children.Add(_widthOrderStrategyLayout);
    }

    private void InitializeNewScreenWidthInput()
    {
        var newScreenWidthLayout = new HorizontalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 2
        };
        
        _newScreenWidthInput = new TextInputBuilder()
            .WithLabel("Add a new screen width")
            .Numeric()
            .WithMaxLength(_maxWidth.ToString().Length)
            .WithValidator(IsValidScreenWidth)
            .Build();
        
        _newScreenWidthInput.HorizontalOptions = LayoutOptions.Fill;
        _widths.ItemRemoved += (sender, e) => _newScreenWidthInput.Revalidate();
        newScreenWidthLayout.Children.Add(_newScreenWidthInput);

        var addNewScreenWidthButton = new Button()
        {
            Text = "+",
            StyleClass = ["AddOrRemoveButton"] // TODO: ADD THIS STYLE CLASS TO STYLES
        };

        addNewScreenWidthButton.Clicked += (sender, e) =>
        {
            if (_newScreenWidthInput.State.IsValid && _widths.Count() < _maxWidthCount)
            {
                var newWidths = new ScreenAndImageWidths
                {
                    ScreenWidth = int.Parse(_newScreenWidthInput.State.Value)
                };

                _widths.Add(newWidths);
                _newScreenWidthInput.Reset();
            }
        };
        
        newScreenWidthLayout.Children.Add(addNewScreenWidthButton);
        RootLayout.Children.Add(newScreenWidthLayout);
    }

    private ValidatorResult IsValidScreenWidth(string value)
    {
        var canParse = int.TryParse(value, out var width);
        if (!canParse || width < _minWidth || width > _maxWidth)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = $"Enter a valid screen width (min. ${_minWidth}, max. ${_maxWidth})."
            };
        }
        
        var isDuplicateScreenWidth = _widths.Any(w => w.ScreenWidth == width);
        return new ValidatorResult
        {
            IsValid = !isDuplicateScreenWidth,
            ErrorMessage = isDuplicateScreenWidth ? "Duplicate screen widths are not allowed." : ""
        };
    }
}