using ImageResizer.Models;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public class ScreenAndImageWidth : IComparable<ScreenAndImageWidth>
{
	public required int ScreenWidth { get; init; }
	public int? ImageWidth { get; set; }

	public int CompareTo(ScreenAndImageWidth other)
	{
		return ScreenWidth.CompareTo(other.ScreenWidth);
	}
}

public struct WidthsInputValue
{
	public int? DefaultWidth { get; init; }
	public WidthComparisonMode WidthComparisonMode { get; init; }
	public IEnumerable<ScreenAndImageWidth> ScreenAndImageWidths { get; init; }
}

public partial class WidthsInput : ContentView, IFormElement<WidthsInputValue>, IFormElementWithErrorDisplay
{
	private static WidthComparisonMode _defaultWidthComparisonMode = WidthComparisonMode.MaxWidths;
    private static int _maxScreenAndImageWidths = 30;
    private static int _minWidth = 1;
    private static int _maxWidth = 40_000;
    
    public event EventHandler<FormElementStateChangedEventArgs<WidthsInputValue>>? StateChanged;
    public FormElementState<WidthsInputValue> State
    {
        get
        {
            int? defaultWidth = null;

            if (_defaultImageWidthInput.State.IsValid &&
                int.TryParse(_defaultImageWidthInput.State.Value, out int temp))
            {
                defaultWidth = temp;
            }

            var isValid = _defaultImageWidthInput.State.IsValid &&
                          ScreenAndImageWidthTextInputs.All(input => input.State.IsValid);
            
            var state = new FormElementState<WidthsInputValue>()
            {
                Value = new WidthsInputValue()
                {
                    DefaultWidth = defaultWidth,
                    ScreenAndImageWidths = _screenAndImageWidths,
                    WidthComparisonMode = WidthComparisonMode
                },
                IsValid = isValid
            };

            return state;
        }
    }

    private WidthComparisonMode WidthComparisonMode
	{
		get;
		set
		{
			field = value;
			_screenAndImageWidths.IsReversed = value == WidthComparisonMode.MinWidths;
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
		}
	} = _defaultWidthComparisonMode;

	private ISortedLiveList<ScreenAndImageWidth> _screenAndImageWidths = new SortedLiveList<ScreenAndImageWidth>()
	{
		IsReversed = _defaultWidthComparisonMode == WidthComparisonMode.MinWidths,
	};

    private IEnumerable<TextInput> ScreenAndImageWidthTextInputs
    {
        get
        {
            return _screenAndImageWidthInputsContainer
                .Children
                .OfType<Layout>()
                .Where(c => c.Children.Any(gc => gc is TextInput))
                .Select(c => (TextInput)c.Children.First(gc => gc is TextInput));
        }
    }

    private TextInput _defaultImageWidthInput;
    private Layout _screenAndImageWidthInputsContainer;

	public WidthsInput()
	{
		InitializeComponent();
        InitInternalComponents();
	}

    public void RevealErrors()
    {
        _defaultImageWidthInput.RevealErrors();

        foreach (var input in ScreenAndImageWidthTextInputs)
        {
            input.RevealErrors();
        }
    }

    private void InitInternalComponents()
	{
		InitializeModeInput();
		InitializeNewScreenWidthInput();
	    InitializeWidthsInputs();
	}

	private void InitializeModeInput()
	{
       var modeInput = new RadioButtonGroup(
       [
           new RadioButtonGroupItem()
            {
                Content = "Max-Widths",
                Value = WidthComparisonMode.MaxWidths.ToString()
            },
            new RadioButtonGroupItem()
            {
                Content = "Min-Widths",
                Value = WidthComparisonMode.MinWidths.ToString()
            },
 
        ], _defaultWidthComparisonMode.ToString(), "WidthComparisonModeGroup")
        {
            LabelText = "Select the media query to use for each screen width:"
        };
 
        modeInput.StateChanged += (sender, e) =>
        {
            WidthComparisonMode = e.State.Value == WidthComparisonMode.MaxWidths.ToString() ? 
				WidthComparisonMode.MaxWidths : 
				WidthComparisonMode.MinWidths;
        };
 
		MainLayout.Children.Add(modeInput);
    }

	private void InitializeNewScreenWidthInput() {
        ValidatorFuncResult ValidateNewScreenWidth(string value)
        {
            var canParse = int.TryParse(value, out var width);
        
            if (canParse && width >= _minWidth && width <= _maxWidth)
            {
                if (_screenAndImageWidths.All(w => w.ScreenWidth != width))
                {
                    return new ValidatorFuncResult(
                        true,
                        ""
                    );
                }
        
                return new ValidatorFuncResult(
                    false,
                    "Duplicate screen width."
                );
            }
        
        
            return new ValidatorFuncResult(
                false,
                $"Please enter a valid screen width (min. {_minWidth}, max. {_maxWidth})."
            );
        }

        var screenWidthInput = new ImageResizer.Views.TextInput
        (
            "",
            ValidateNewScreenWidth,
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Add a new screen width:",
            MaxLength = _maxWidth.ToString().Length
        };

        var addScreenWidthButton = new Button()
        {
            Text = "+"
        };
        
        addScreenWidthButton.Clicked += (sender, e) =>
        {
            if (_screenAndImageWidths.Count() >= _maxScreenAndImageWidths) return; 
            
            if (screenWidthInput.State.IsValid && int.TryParse(screenWidthInput.Value, out var width))
            {
                _screenAndImageWidths.Add(new ScreenAndImageWidth { ScreenWidth = width });
                screenWidthInput.Reset();
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
            }
            else
            {
                screenWidthInput.RevealErrors();
            }
        };
        
        void toggleAddScreenWidthButton_IsEnabled()
        {
            addScreenWidthButton.IsEnabled = _screenAndImageWidths.Count() <=  _maxScreenAndImageWidths;
        }
        
        toggleAddScreenWidthButton_IsEnabled();
        
        _screenAndImageWidths.ItemAdded += (sender, e) =>
        {
            toggleAddScreenWidthButton_IsEnabled();
        };

        _screenAndImageWidths.ItemRemoved += (sender, e) =>
        {
           toggleAddScreenWidthButton_IsEnabled();
        };
        
        var screenWidthInputContainer = new HorizontalStackLayout();
        screenWidthInputContainer.Children.Add(screenWidthInput);
        screenWidthInputContainer.Children.Add(addScreenWidthButton);
        MainLayout.Children.Add(screenWidthInputContainer);
	}

	private void InitializeWidthsInputs()
    {
        var validateImageWidth = FormElementHelpers.CreateMinMaxValidator
        (
            _minWidth,
            _maxWidth,
            $"Please enter a valid image width (min. {_minWidth}, max. {_maxWidth})."
        );
        
        _defaultImageWidthInput = new TextInput
        (
            "",
            validateImageWidth,
            FormElementHelpers.AllowOnlyDigits
        )
        {
            MaxLength = _maxWidth.ToString().Length
        };

        _defaultImageWidthInput.StateChanged += (sender, e) =>
        {
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
        };

        var defaultScreenWidthLabel = new Label()
        {
            Text = "Default"
        };

        var defaultScreenWidthInputContainer = new HorizontalStackLayout();
        defaultScreenWidthInputContainer.Children.Add(defaultScreenWidthLabel);
        defaultScreenWidthInputContainer.Children.Add(_defaultImageWidthInput);
        
        _screenAndImageWidthInputsContainer = new VerticalStackLayout();
        
        DynamicListFactory.MakeDynamic<ScreenAndImageWidth>(_screenAndImageWidthInputsContainer, _screenAndImageWidths, (screenAndImageWidth) =>
        {
            var widthInputContainer = new HorizontalStackLayout();
            var widthInputLabel = new Label()
            {
                Text = screenAndImageWidth.ScreenWidth.ToString()
            };
        
            widthInputContainer.Children.Add(widthInputLabel);
        
            var input = new TextInput
            (
                "",
                validateImageWidth,
                FormElementHelpers.AllowOnlyDigits
            )
            {
                MaxLength = _maxWidth.ToString().Length
            };
        
            input.StateChanged += (sender, e) =>
            {
                if (e.State.IsValid && int.TryParse(e.State.Value, out var width))
                {
                    screenAndImageWidth.ImageWidth = width;
                }
                else
                {
                    screenAndImageWidth.ImageWidth = null;
                }
                
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
            };
        
            widthInputContainer.Children.Add(input);
        
            var removeWidthButton = new Button()
            {
                Text = "-"
            };
        
            removeWidthButton.Clicked += (sender, e) =>
            {
                _screenAndImageWidths.Remove(screenAndImageWidth);
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
            };
        
            widthInputContainer.Children.Add(removeWidthButton);
        
        
            return widthInputContainer;
        });


        var outerContainer = new VerticalStackLayout();
        outerContainer.Children.Add(_screenAndImageWidthInputsContainer);
        outerContainer.Children.Add(defaultScreenWidthInputContainer);
        MainLayout.Children.Add(outerContainer);
    }
}