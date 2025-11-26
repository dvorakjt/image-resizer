using ImageResizer.DataModel;
using ImageResizer.DataModel.ResponsiveImageSettings;
using ImageResizer.FormControls;
using ImageResizer.Utils;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class MediaQueriesFormGroup : ContentView, IFormElement<MediaQueriesFormGroupValue>
{
    public event EventHandler<IFormElementState<MediaQueriesFormGroupValue>>? StateChanged;

    public IFormElementState<MediaQueriesFormGroupValue> State
    {
        get
        {
            var isValid = _defaultImageWidthInput.State.IsValid &&  GetAllTextInputs().All(i => i.State.IsValid);
            int? defaultImageWidth = null;

            if (int.TryParse(_defaultImageWidthInput.State.Value, out int w))
            {
                defaultImageWidth = w;
            }

            return new FormElementState<MediaQueriesFormGroupValue>
            {
                Value = new MediaQueriesFormGroupValue
                {
                    DefaultImageWidth = defaultImageWidth,
                    MediaQueryAndImageWidths = _mediaQueries.Select(mq => new MediaQueryAndImageWidth
                    {
                        MediaQuery = mq.MediaQuery,
                        ImageWidth = mq.ImageWidth
                    })
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
    private IPrependableLiveList<MediaQueryAndImageWidth>  _mediaQueries = new PrependableLiveList<MediaQueryAndImageWidth>();
    private TextInput _defaultImageWidthInput;
    private VerticalStackLayout _mediaQueriesListLayout;
    private readonly int _minImageWidth = 1;
    private readonly int _maxImageWidth = 40_000;
    private readonly int _maxQueryCount = 30;
    
    public MediaQueriesFormGroup()
    {
        InitializeComponent();
        InitializeMediaQueriesSection();
        SubscribeToMediaQueriesList();
    }
    
    public void DisplayErrors()
    {
        _defaultImageWidthInput.DisplayErrors();
        foreach (var input in GetAllTextInputs())
        {
            input.DisplayErrors();
        }
    }

    public void Revalidate()
    {
        _defaultImageWidthInput.Revalidate();
        foreach (var input in GetAllTextInputs())
        {
            input.Revalidate();
        }
    }

    public void Reset()
    {
        _mediaQueries.Clear();
        _defaultImageWidthInput.Reset();
    }

    private void SubscribeToMediaQueriesList()
    {
        _mediaQueries.ItemAdded += 
            (sender, e) => StateChanged?.Invoke(this, State);
        _mediaQueries.ItemRemoved +=
            (sender, e) => StateChanged?.Invoke(this, State);
        _mediaQueries.ListReset +=
            (sender, e) => StateChanged?.Invoke(this, State);
    }

    private void InitializeMediaQueriesSection()
    {
        var outerLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };

        var header = new HorizontalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 2
        };

        var mediaQueryHeading = new Label()
        {
            Text = "Media Query",
            WidthRequest = 304
        };

        var imageWidthHeading = new Label()
        {
            Text = "Image Width",
        };
        
        header.Children.Add(mediaQueryHeading);
        header.Children.Add(imageWidthHeading);
        outerLayout.Children.Add(header);

        _mediaQueriesListLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };
        
        DynamicListFactory.MakeDynamic(_mediaQueriesListLayout, _mediaQueries, (mediaQuery) =>
        {
            var row = new HorizontalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Fill,
                Spacing = 2
            };

            var mediaQueryInput = new TextInputBuilder()
                .WithValidator(FormControlHelpers.CreateRequiredFieldValidator("Please enter a media query."))
                .WithWidthRequest(304)
                .Build();

            mediaQueryInput.StateChanged += (sender, state) =>
            {
                mediaQuery.MediaQuery = state.Value;
                StateChanged?.Invoke(this, State);
            };
            
            row.Children.Add(mediaQueryInput);

            var imageWidthValidator = FormControlHelpers.ChainValidators
            (
                FormControlHelpers.CreateRequiredFieldValidator("Required"),
                FormControlHelpers.CreateMinMaxValidator(_minImageWidth, _maxImageWidth, $"Min. {_minImageWidth}, max. {_maxImageWidth}")
            );
            
            var imageWidthInput = new TextInputBuilder()
                .PositiveIntegersOnly()
                .WithValidator(
                    imageWidthValidator
                    )
                .WithMaxLength(_maxImageWidth.ToString().Length)
                .WithWidthRequest(100)
                .Build();

            imageWidthInput.StateChanged += (sender, state) =>
            {
                mediaQuery.ImageWidth = state.IsValid ? int.Parse(state.Value) : null;
                StateChanged?.Invoke(this, State);
            };  
            
            row.Children.Add(imageWidthInput);

            var removeQueryButton = new Button()
            {
                Text = "-",
                StyleClass = ["AddOrRemoveButton"],
                VerticalOptions = LayoutOptions.Start,
            };

            removeQueryButton.Clicked += (sender, e) => _mediaQueries.Remove(mediaQuery);
            row.Children.Add(removeQueryButton);
            return row;
        });
        
        outerLayout.Children.Add(_mediaQueriesListLayout);

        _defaultImageWidthInput = new TextInputBuilder()
            .WithLabel("Default Image Width")
            .PositiveIntegersOnly()
            .WithMaxLength(_maxImageWidth.ToString().Length)
            .WithValidator
            (
                FormControlHelpers.CreateMinMaxValidator
                (
                    _minImageWidth,
                    _maxImageWidth,
                    $"Please enter a number between {_minImageWidth} and {_maxImageWidth}"
                )
            ).Build();
        
        _defaultImageWidthInput.StateChanged += (sender, state) => StateChanged?.Invoke(this, State);
        outerLayout.Children.Add(_defaultImageWidthInput);
        RootLayout.Children.Add(outerLayout);
    }
    

    private void AddMediaQuery(object sender, EventArgs e)
    {
        if (_mediaQueries.Count() < _maxQueryCount)
        {
            var newMediaQuery = new MediaQueryAndImageWidth();
            _mediaQueries.Prepend(newMediaQuery);
        }
    }

    private IEnumerable<TextInput> GetAllTextInputs()
    {
        IList<TextInput> inputs = new List<TextInput>();

        foreach (var el1 in _mediaQueriesListLayout.Children)
        {
            if (el1 is Layout row)
            {
                foreach (var el2 in row.Children)
                {
                    if(el2 is TextInput input) inputs.Add(input);
                }
            }
        }

        return inputs;
    }
}