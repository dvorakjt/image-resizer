using ImageResizer.FormGroups.Formats;
using ImageResizer.FormGroups.Output;
using ImageResizer.FormGroups.ResponsiveImageSettings;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    private ResponsiveImageSettingsFormGroup _responsiveImageSettingsFormGroup;
    private FormatsFormGroup _formatsFormGroup;
    private OutputFormGroup _outputFormGroup;
    
    public MainPage()
    {
        InitializeComponent();
        InitializeFormGroups();
        InitializeResizeButton();
        InitializeResetButton();
    }
    
    private void InitializeFormGroups()
    {
        _responsiveImageSettingsFormGroup = new ResponsiveImageSettingsFormGroup();
        RootLayout.Children.Add(_responsiveImageSettingsFormGroup);
        
        _formatsFormGroup = new FormatsFormGroup();
        RootLayout.Children.Add(_formatsFormGroup);

        _outputFormGroup = new OutputFormGroup();
        RootLayout.Children.Add(_outputFormGroup);
    }

    private void InitializeResizeButton()
    {
        var resizeButton = new Button()
        {
            Text = "Resize",
            StyleClass = ["LargeButton"]
        };
        
        RootLayout.Children.Add(resizeButton);
    }

    private void InitializeResetButton()
    {
        var resetButton = new Button()
        {
            Text = "Reset",
            StyleClass = ["LargeButton", "SecondaryButton"]
        };

        resetButton.Clicked += (sender, args) => Reset();

        RootLayout.Children.Add(resetButton);
    }

    private async Task Reset()
    {
        var shouldReset = await DisplayAlert("Confirm", "Are you sure you would like to reset the form?", "Reset", "Cancel");
        
        if (shouldReset)
        {
            _responsiveImageSettingsFormGroup.Reset();
            _formatsFormGroup.Reset();
            _outputFormGroup.Reset();
        }

        await ScrollContainer.ScrollToAsync(0.0d, 0.0d, false);
    }
}