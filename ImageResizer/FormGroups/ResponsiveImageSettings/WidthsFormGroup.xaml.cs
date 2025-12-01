using ImageResizer.DataModel;
using ImageResizer.DataModel.ResponsiveImageSettings;
using ImageResizer.FormControls;
using ImageResizer.Utils;
using Microsoft.Maui.Layouts;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class WidthsFormGroup : ContentView, IFormElement<WidthsFormGroupValue>
{
    
    public event EventHandler<IFormElementState<WidthsFormGroupValue>>? StateChanged;

    public IFormElementState<WidthsFormGroupValue> State
    {
        get
        {
            WidthThresholdsStrategy widthThresholdsStrategy;

            if (_widthThresholdsStrategyRadioGroup.State.Value == WidthThresholdsStrategy.MaxWidths.ToString())
            {
                widthThresholdsStrategy = WidthThresholdsStrategy.MaxWidths;
            } 
            else if(_widthThresholdsStrategyRadioGroup.State.Value == WidthThresholdsStrategy.MinWidths.ToString())
            {
                widthThresholdsStrategy = WidthThresholdsStrategy.MinWidths;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported width threshold strategy: {_widthThresholdsStrategyRadioGroup.State.Value}");
            }
            
            int? defaultImageWidth = null;
            if (int.TryParse(_defaultWidthInput.State.Value, out int w))
            {
                defaultImageWidth = w;
            }
            
            var isValid = _defaultWidthInput.State.IsValid && GetAllTextInputs().All(i => i.State.IsValid);

            return new FormElementState<WidthsFormGroupValue>
            {
                Value = new WidthsFormGroupValue
                {
                    WidthThresholdsStrategy = widthThresholdsStrategy,
                    ScreenAndImageWidths = _widths.Select(w => new ScreenAndImageWidths
                    {
                        ScreenWidth = w.ScreenWidth,
                        ImageWidth = w.ImageWidth,
                    }),
                    DefaultImageWidth = defaultImageWidth,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
    private CustomRadioButtonGroup _widthThresholdsStrategyRadioGroup;
    private TextInput _newScreenWidthInput;
    private ISortedLiveList<ScreenAndImageWidths> _widths = new SortedLiveList<ScreenAndImageWidths>();
    private TextInput _defaultWidthInput;
    private VerticalStackLayout _widthsListLayout;
    private readonly int _minWidth = 1;
    private readonly int _maxWidth = 40_000;
    private readonly int _maxWidthCount = 30;
    
    public WidthsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
        SubscribeToWidths();
    }
    
    public void DisplayErrors()
    {
        _defaultWidthInput.DisplayErrors();
        _widthThresholdsStrategyRadioGroup.DisplayErrors();
        foreach (var input in GetAllTextInputs())
        {
            input.DisplayErrors();
        }
    }
    
    public void Revalidate()
    {
        _defaultWidthInput.Revalidate();
        _widthThresholdsStrategyRadioGroup.Revalidate();
        foreach (var input in GetAllTextInputs())
        {
            input.Revalidate();
        }
    }
    
    public void Reset()
    {
        _newScreenWidthInput.Reset();
        _widthThresholdsStrategyRadioGroup.Reset();
        _widths.Clear();
        _defaultWidthInput.Reset();
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
        {
            _widths.IsReversed = _widthThresholdsStrategyRadioGroup.State.Value ==
                                 WidthThresholdsStrategy.MinWidths.ToString();

            StateChanged?.Invoke(this, State);
        };
        
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
            .DiplayErrorsOnCommandOnly()
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

        _widthsListLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };
        
        DynamicListFactory.MakeDynamic(_widthsListLayout, _widths, (width) =>
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
                .WithDefaultValue(width.ImageWidth.HasValue ? width.ImageWidth.Value.ToString() : "")
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
                StateChanged?.Invoke(this, State);
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
        
        outerLayout.Children.Add(_widthsListLayout);

        _defaultWidthInput = new TextInputBuilder()
            .WithLabel("Default Image Width")
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
        
        _defaultWidthInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        outerLayout.Children.Add(_defaultWidthInput);
        RootLayout.Children.Add(outerLayout);
    }
    
    private void SubscribeToWidths()
    {
        _widths.ItemAdded += (sender, e) => StateChanged?.Invoke(this, State);
        _widths.ItemRemoved += (sender, e) => StateChanged?.Invoke(this, State);
        _widths.ListReset += (sender, e) => StateChanged?.Invoke(this, State);
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

    private IEnumerable<TextInput> GetAllTextInputs()
    {
        IList<TextInput> inputs = new List<TextInput>();

        foreach (var el1 in _widthsListLayout.Children)
        {
            if (el1 is Layout row)
            {
                foreach (var el2 in row.Children)
                {
                    if (el2 is Layout cell)
                    {
                        foreach (var el3 in cell.Children)
                        {
                            if(el3 is TextInput textInput) inputs.Add(textInput);
                        }
                    }
                }
            }
        }
        
        return inputs;
    }
}