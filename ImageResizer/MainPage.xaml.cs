using ImageResizer.FormControls;
using ImageResizer.DataModel;
using ImageResizer.FormGroups.Formats;
using ImageResizer.FormGroups.Output;
using ImageResizer.FormGroups.ResponsiveImageSettings;

namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var formatsFormGroup = new FormatsFormGroup();
        MainLayout.Children.Add(formatsFormGroup);
        
        var responsiveImageSettingsFormGroup = new ResponsiveImageSettingsFormGroup();
        MainLayout.Children.Add(responsiveImageSettingsFormGroup);

        var outputFormGroup = new OutputFormGroup();
        MainLayout.Children.Add(outputFormGroup);
    }
}