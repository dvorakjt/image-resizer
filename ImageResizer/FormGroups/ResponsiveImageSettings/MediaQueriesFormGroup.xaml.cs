using ImageResizer.DataModel;
using ImageResizer.FormControls;
using ImageResizer.Utils;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class MediaQueriesFormGroup : ContentView
{
    private IPrependableLiveList<MediaQueryAndImageWidth>  _mediaQueries = new PrependableLiveList<MediaQueryAndImageWidth>();
    private TextInput _defaultImageWidthInput;
    private readonly int _minImageWidth = 1;
    private readonly int _maxImageWidth = 40_000;
    private readonly int _maxQueryCount = 30;
    
    public MediaQueriesFormGroup()
    {
        InitializeComponent();
        InitializeMediaQueriesSection();
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

        var mediaQueriesListLayout = new VerticalStackLayout()
        {
            HorizontalOptions = LayoutOptions.Fill,
            Spacing = 5
        };
        
        DynamicListFactory.MakeDynamic(mediaQueriesListLayout, _mediaQueries, (mediaQuery) =>
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
        
        outerLayout.Children.Add(mediaQueriesListLayout);

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
}