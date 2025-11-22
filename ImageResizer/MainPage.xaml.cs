using ImageResizer.FormGroups.Formats;
using ImageResizer.FormGroups.Output;
using ImageResizer.FormGroups.ResponsiveImageSettings;

namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        InitializeFormGroups();
        InitializeResizeButton();
        InitializeResetButton();
    }
    
    private void InitializeFormGroups()
    {
        var responsiveImageSettingsFormGroup = new ResponsiveImageSettingsFormGroup();
        RootLayout.Children.Add(responsiveImageSettingsFormGroup);
        
        var formatsFormGroup = new FormatsFormGroup();
        RootLayout.Children.Add(formatsFormGroup);

        var outputFormGroup = new OutputFormGroup();
        RootLayout.Children.Add(outputFormGroup);
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
            // call reset methods of each form group
            // scroll to top
        }
    }
}