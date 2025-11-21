using ImageResizer.DataModel;
using ImageResizer.FormControls;
using ImageResizer.Utils;
using Microsoft.Maui.Layouts;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class WidthsFormGroup : ContentView
{
    private CustomRadioButtonGroup _widthThresholdsStrategyRadioGroup;
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
       InitializeWidthThresholdsStrategyRadioGroup();
       InitializeNewScreenWidthInput();
       InitializeWidthsList();
    }

    private void InitializeWidthThresholdsStrategyRadioGroup()
    {
        var widthThrehsholdsStrategyLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };

        var label = new Label()
        {
            Text = "Width Thresholds Strategy",
        };
        
        widthThrehsholdsStrategyLayout.Children.Add(label);

        _widthThresholdsStrategyRadioGroup = new CustomRadioButtonGroup
        (
            [
                new CustomRadioButtonGroupItem
                {
                    Content = "Max-Widths",
                    Value = WidthThresholdsStrategy.MaxWidths.ToString(),

                },
                new CustomRadioButtonGroupItem
                {
                    Content = "Min-Widths",
                    Value = WidthThresholdsStrategy.MinWidths.ToString(),
                }
            ],
            WidthThresholdsStrategy.MaxWidths.ToString(),
            "WidthThresholdsStrategy",
            18
        );

        _widths.IsReversed = _widthThresholdsStrategyRadioGroup.State.Value ==
                             WidthThresholdsStrategy.MinWidths.ToString();
        
        _widthThresholdsStrategyRadioGroup.StateChanged += (sender, e) =>
            _widths.IsReversed = _widthThresholdsStrategyRadioGroup.State.Value ==
                                 WidthThresholdsStrategy.MinWidths.ToString();
        
        widthThrehsholdsStrategyLayout.Children.Add(_widthThresholdsStrategyRadioGroup);
        RootLayout.Children.Add(widthThrehsholdsStrategyLayout);
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
            .PositiveIntegersOnly()
            .WithMaxLength(_maxWidth.ToString().Length)
            .WithValidator(IsValidScreenWidth)
            .Build();

        _newScreenWidthInput.Completed += (sender, e) =>
        {
            AddNewScreenWidth();
            _newScreenWidthInput.Focus();
        };
        
        _widths.ItemRemoved += (sender, e) => _newScreenWidthInput.Revalidate();
        _newScreenWidthInput.WidthRequest = 406;
        newScreenWidthLayout.Children.Add(_newScreenWidthInput);

        var addNewScreenWidthButton = new Button()
        {
            Text = "+",
            StyleClass = ["AddOrRemoveButton"]
        };

        addNewScreenWidthButton.Clicked += (sender, e) => AddNewScreenWidth();
        addNewScreenWidthButton.VerticalOptions = LayoutOptions.Start;
        addNewScreenWidthButton.Margin = new Thickness(0, 18, 0, 0);
        newScreenWidthLayout.Children.Add(addNewScreenWidthButton);
        RootLayout.Children.Add(newScreenWidthLayout);
    }

    private void InitializeWidthsList()
    {
        var outerLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };

        var header = new HorizontalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 36
        };

        var screenWidthsColumnHeading = new Label()
        {
            Text = "Screen Width",
            WidthRequest = 96
        };
        header.Children.Add(screenWidthsColumnHeading);
        
        
        var imageWidthsColumnHeading = new Label()
        {
            Text = "Image Width",
        };
        header.Children.Add(imageWidthsColumnHeading);
        outerLayout.Children.Add(header);

        var scrollView = new ScrollView()
        {
            MaximumHeightRequest = 145,
            HorizontalOptions = LayoutOptions.Fill,
        };

        var screenWidthsList = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };
        
        scrollView.Content = screenWidthsList;
        
        DynamicListFactory.MakeDynamic(screenWidthsList, _widths, (width) =>
        {
            var row = new HorizontalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Fill,
                Spacing = 36
            };

            var screenWidth = new Label()
            {
                Text = width.ScreenWidth.ToString(),
                WidthRequest = 96,
                HeightRequest = 32,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            
            row.Children.Add(screenWidth);

            var imageWidthCell = new HorizontalStackLayout()
            {
                WidthRequest = 308,
                Spacing = 2
            };
            
            row.Children.Add(imageWidthCell);

            var imageWidthInput = new TextInputBuilder()
                .PositiveIntegersOnly()
                .WithValidator(
                    FormControlHelpers.CreateMinMaxValidator
                    (
                        _minWidth,
                        _maxWidth,
                        $"Enter a valid image width (min. {_minWidth}, max. {_maxWidth})"
                    )
                )
                .WithMaxLength(_maxWidth.ToString().Length)
                .WithWidthRequest(274)
                .Build();

            imageWidthInput.StateChanged += (sender, e) =>
            {
                width.ImageWidth = imageWidthInput.State.IsValid ? int.Parse(imageWidthInput.State.Value) : null;
            };
            
            imageWidthCell.Children.Add(imageWidthInput);

            var removeWidthButton = new Button()
            {
                Text = "-",
                StyleClass = ["AddOrRemoveButton"],
                VerticalOptions = LayoutOptions.Start
            };
            
            removeWidthButton.Clicked +=  (sender, e) => _widths.Remove(width);
            imageWidthCell.Children.Add(removeWidthButton);
            return row;
        });
        
        outerLayout.Children.Add(scrollView);

        var defaultWidthInput = new TextInputBuilder()
            .WithLabel("Default")
            .PositiveIntegersOnly()
            .WithValidator(
                FormControlHelpers.CreateMinMaxValidator(
                    _minWidth,
                    _maxWidth,
                    $"Please enter a valid image width (min. {_minWidth}, max. {_maxWidth})"
                )
            )
            .WithMaxLength(_maxWidth.ToString().Length)
            .Build();
        
        outerLayout.Children.Add(defaultWidthInput);
        RootLayout.Children.Add(outerLayout);
    }

    private ValidatorResult IsValidScreenWidth(string value)
    {
        var canParse = int.TryParse(value, out var width);
        if (!canParse || width < _minWidth || width > _maxWidth)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = $"Enter a valid screen width (min. {_minWidth}, max. {_maxWidth})"
            };
        }
        
        var isDuplicateScreenWidth = _widths.Any(w => w.ScreenWidth == width);
        return new ValidatorResult
        {
            IsValid = !isDuplicateScreenWidth,
            ErrorMessage = isDuplicateScreenWidth ? "Duplicate screen widths are not allowed." : ""
        };
    }
    
    private void AddNewScreenWidth()
    {
        if (_widths.Count() >= _maxWidthCount) return;
        
        if (_newScreenWidthInput.State.IsValid)
        {
            var newWidths = new ScreenAndImageWidths
            {
                ScreenWidth = int.Parse(_newScreenWidthInput.State.Value)
            };

            _widths.Add(newWidths);
            _newScreenWidthInput.Reset();
        }
        else
        {
            _newScreenWidthInput.DisplayErrors();
        }
    }
}