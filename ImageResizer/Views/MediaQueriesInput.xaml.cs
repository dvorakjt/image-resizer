using ImageResizer.Models;
using ImageResizer.ViewModels;
using Microsoft.Maui.Layouts;

namespace ImageResizer.Views;

public class MediaQueryAndImageWidth
{
	public string MediaQuery { get; set; }
	public int? ImageWidth { get; set; }
}

public struct MediaQueriesInputValue
{
	public int? DefaultWidth { get; init; }
	public IEnumerable<MediaQueryAndImageWidth> MediaQueriesAndImageWidths { get; init; }
}

public partial class MediaQueriesInput : ContentView, IFormElement<MediaQueriesInputValue>, IFormElementWithErrorDisplay
{
    private static int _maxQueries = 30;
    private static int _minWidth = 1;
    private static int _maxWidth = 40_000;
    private static int _addAndRemoveButtonSize = 46;
    private static int _marginBetweenInputElements = 2;
    
    public event EventHandler<FormElementStateChangedEventArgs<MediaQueriesInputValue>>? StateChanged;
    public FormElementState<MediaQueriesInputValue> State
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
                          MediaQueryAndWidthInputs.All(input => input.State.IsValid);
            
            var state = new FormElementState<MediaQueriesInputValue>()
            {
                Value = new MediaQueriesInputValue()
                {
                    DefaultWidth = defaultWidth,
                    MediaQueriesAndImageWidths = _mediaQueriesAndImageWidths,
                },
                IsValid = isValid
            };

            return state;
        }
    }

    private ILiveList<MediaQueryAndImageWidth> _mediaQueriesAndImageWidths =
        new ReorderableLiveList<MediaQueryAndImageWidth>();
    
    private IEnumerable<TextInput> MediaQueryAndWidthInputs
    {
        get
        {
            return _mediaQueryAndWidthInputsContainer
                .Children
                .OfType<Layout>()
                .Where(c => c.Children.Any(gc => gc is TextInput))
                .SelectMany(c => c.Children.OfType<TextInput>());
        }
    }

    private TextInput _defaultImageWidthInput;
    private Layout _mediaQueryAndWidthInputsContainer;

	public MediaQueriesInput()
	{
		InitializeComponent();
        SetWidth();
        InitHeader();
        InitInputElements();
	}

    public void RevealErrors()
    {
        _defaultImageWidthInput.RevealErrors();

        foreach (var input in MediaQueryAndWidthInputs)
        {
            input.RevealErrors();
        }
    }

    private void SetWidth()
    {
        MainLayout.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        MainLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
    }

    private void InitHeader()
    {
        var heading = new Label()
        {
            Text = "Media Queries",
            StyleClass = ["SubHeading2"]
        };

        var addMediaQueryButton = new Button()
        {
            Text = "Add New"
        };

        addMediaQueryButton.Clicked += (sender, args) =>
        {
            if (_mediaQueriesAndImageWidths.Count() >= _maxQueries) return;
            
            var mediaQuery = new MediaQueryAndImageWidth();
            _mediaQueriesAndImageWidths.Add(mediaQuery);
            _mediaQueriesAndImageWidths.Move(mediaQuery, 0);
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<MediaQueriesInputValue>(State));
        };
        
        void toggleAddMediaQueryButton_IsEnabled()
        {
            addMediaQueryButton.IsEnabled = _mediaQueriesAndImageWidths.Count() <=  _maxQueries;
        }
        
        toggleAddMediaQueryButton_IsEnabled();
        
        _mediaQueriesAndImageWidths.ItemAdded += (sender, e) =>
        {
            toggleAddMediaQueryButton_IsEnabled();
        };

        _mediaQueriesAndImageWidths.ItemRemoved += (sender, e) =>
        {
            toggleAddMediaQueryButton_IsEnabled();
        };

        var header = new FlexLayout()
        {
            JustifyContent = FlexJustify.SpaceBetween,
            Margin = new Thickness(0, 0, 0, 10)
        };

        header.Children.Add(heading);
        header.Children.Add(addMediaQueryButton);
        MainLayout.Children.Add(header);
    }

	private void InitInputElements() 
    {
        Func<string, ValidatorFuncResult> validateImageWidth = (value) =>
        {
            var canParse = int.TryParse(value, out var width);
            bool isValid = canParse && width >= _minWidth && width <= _maxWidth;
 
            return new ValidatorFuncResult(
                isValid,
                isValid ? "" : $"Invalid: min. {_minWidth}, max. {_maxWidth}"
            );
        };

        var mediaQueryInputWidth = 310;
        var screenWidthInputWidth = 
            AppDimensions.CONTENT_WIDTH - mediaQueryInputWidth - _addAndRemoveButtonSize - 2 * _marginBetweenInputElements;
        
        var defaultScreenWidthLabel = new Label()
        {
            Text = "Default",
            MinimumWidthRequest = mediaQueryInputWidth,
            MaximumWidthRequest = mediaQueryInputWidth,
            Margin = new Thickness(0, 0, _marginBetweenInputElements, 0)
        };
        
        _defaultImageWidthInput = new TextInput("", validateImageWidth, FormElementHelpers.AllowOnlyDigits)
        {
            MaxLength = _maxWidth.ToString().Length,
            MinimumWidthRequest = screenWidthInputWidth,
            MaximumWidthRequest = screenWidthInputWidth
        };

        _defaultImageWidthInput.StateChanged += (sender, e) =>
        {
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<MediaQueriesInputValue>(State));
        };

        var defaultScreenWidthInputContainer = new HorizontalStackLayout();
        defaultScreenWidthInputContainer.Children.Add(defaultScreenWidthLabel);
        defaultScreenWidthInputContainer.Children.Add(_defaultImageWidthInput);
        
        _mediaQueryAndWidthInputsContainer = new VerticalStackLayout();
        
        DynamicListFactory.MakeDynamic<MediaQueryAndImageWidth>(_mediaQueryAndWidthInputsContainer, _mediaQueriesAndImageWidths, (mediaQueryAndImageWidth) =>
        {
            var mediaQueryAndImageWidthInputContainer = new HorizontalStackLayout();
            var mediaQueryInput = new TextInput(mediaQueryAndImageWidth.MediaQuery,
                FormElementHelpers.CreateRequiredFieldValidator("Please enter a media query."))
            {
                MaxLength = 255,
                MinimumWidthRequest = mediaQueryInputWidth,
                MaximumWidthRequest = mediaQueryInputWidth,
                Margin = new Thickness(0, 0, _marginBetweenInputElements, 0)
            };

            mediaQueryInput.StateChanged += (sender, e) =>
            {
                mediaQueryAndImageWidth.MediaQuery = mediaQueryInput.State.Value;
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<MediaQueriesInputValue>(State));
            };
        
            mediaQueryAndImageWidthInputContainer.Children.Add(mediaQueryInput);
        
            var imageWidthInput = new TextInput("", validateImageWidth, FormElementHelpers.AllowOnlyDigits)
            {
                MaxLength = _maxWidth.ToString().Length,
                MinimumWidthRequest = screenWidthInputWidth,
                MaximumWidthRequest = screenWidthInputWidth,
                Margin = new Thickness(0, 0, _marginBetweenInputElements, 0)
            };
        
            imageWidthInput.StateChanged += (sender, e) =>
            {
                if (e.State.IsValid && int.TryParse(e.State.Value, out var width))
                {
                    mediaQueryAndImageWidth.ImageWidth = width;
                }
                else
                {
                    mediaQueryAndImageWidth.ImageWidth = null;
                }
                
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<MediaQueriesInputValue>(State));
            };
        
            mediaQueryAndImageWidthInputContainer.Children.Add(imageWidthInput);
            
            var removeItemButton = new Button()
            {
                Text = "-",
                MinimumWidthRequest = _addAndRemoveButtonSize,
                MaximumWidthRequest = _addAndRemoveButtonSize,
                MinimumHeightRequest = _addAndRemoveButtonSize,
                MaximumHeightRequest = _addAndRemoveButtonSize,
                VerticalOptions = LayoutOptions.Start
            };
        
            removeItemButton.Clicked += (sender, e) =>
            {
                _mediaQueriesAndImageWidths.Remove(mediaQueryAndImageWidth);
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<MediaQueriesInputValue>(State));
            };
        
            mediaQueryAndImageWidthInputContainer.Children.Add(removeItemButton);
            
            return mediaQueryAndImageWidthInputContainer;
        });


        var outerContainer = new VerticalStackLayout();
        outerContainer.Children.Add(_mediaQueryAndWidthInputsContainer);
        outerContainer.Children.Add(defaultScreenWidthInputContainer);
        MainLayout.Children.Add(outerContainer);
    }
}