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

public partial class WidthsInput : ContentView
{
	private static WidthComparisonMode _defaultWidthComparisonMode = WidthComparisonMode.MaxWidths;
    private static int _minWidth = 1;
    private static int _maxWidth = 40_000;

	private WidthComparisonMode WidthComparisonMode
	{
		get;
		set
		{
			field = value;
			_screenAndImageWidths.IsReversed = value == WidthComparisonMode.MaxWidths;
		}
	} = _defaultWidthComparisonMode;

	private ISortedLiveList<ScreenAndImageWidth> _screenAndImageWidths = new SortedLiveList<ScreenAndImageWidth>()
	{
		IsReversed = _defaultWidthComparisonMode == WidthComparisonMode.MaxWidths
	};

	public WidthsInput()
	{
		InitializeComponent();
        InitInternalComponents();
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
            if (screenWidthInput.State.IsValid && int.TryParse(screenWidthInput.Value, out var width))
            {
                _screenAndImageWidths.Add(new ScreenAndImageWidth { ScreenWidth = width });
                screenWidthInput.Reset();
            }
            else
            {
                screenWidthInput.RevealErrors();
            }
        };


        var screenWidthInputContainer = new HorizontalStackLayout();
        screenWidthInputContainer.Children.Add(screenWidthInput);
        screenWidthInputContainer.Children.Add(addScreenWidthButton);
        MainLayout.Children.Add(screenWidthInputContainer);
	}

	private void InitializeWidthsInputs() 
    {
        Func<string, ValidatorFuncResult> validateImageWidth = (value) =>
        {
            var canParse = int.TryParse(value, out var width);
            bool isValid = canParse && width >= _minWidth && width <= _maxWidth;
 
            return new ValidatorFuncResult(
                isValid,
                isValid ? "" : $"Please enter a valid image width (min. {_minWidth}, max. {_maxWidth})."
            );
 
        };
        
        var defaultScreenWidthInput = new TextInput("", validateImageWidth, FormElementHelpers.AllowOnlyDigits)
        {
            MaxLength = _maxWidth.ToString().Length
        };

        var defaultScreenWidthLabel = new Label()
        {
            Text = "Default"
        };

        var defaultScreenWidthInputContainer = new HorizontalStackLayout();
        defaultScreenWidthInputContainer.Children.Add(defaultScreenWidthLabel);
        defaultScreenWidthInputContainer.Children.Add(defaultScreenWidthInput);
        
        var screenWidthInputsContainer = new VerticalStackLayout();
        
        DynamicListFactory.MakeDynamic<ScreenAndImageWidth>(screenWidthInputsContainer, _screenAndImageWidths, (screenAndImageWidth) =>
        {
            var widthInputContainer = new HorizontalStackLayout();
            var widthInputLabel = new Label()
            {
                Text = screenAndImageWidth.ScreenWidth.ToString()
            };
        
            widthInputContainer.Children.Add(widthInputLabel);
        
            var input = new TextInput("", validateImageWidth, FormElementHelpers.AllowOnlyDigits)
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
            };
        
            widthInputContainer.Children.Add(input);
        
            var removeWidthButton = new Button()
            {
                Text = "-"
            };
        
            removeWidthButton.Clicked += (sender, e) =>
            {
                _screenAndImageWidths.Remove(screenAndImageWidth);
            };
        
            widthInputContainer.Children.Add(removeWidthButton);
        
        
            return widthInputContainer;
        });


        var outerContainer = new VerticalStackLayout();
        outerContainer.Children.Add(defaultScreenWidthInputContainer);
        outerContainer.Children.Add(screenWidthInputsContainer);
        MainLayout.Children.Add(outerContainer);
    }
}