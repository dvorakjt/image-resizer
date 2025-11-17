using ImageResizer.ViewFactories;
using ImageResizer.ViewModels;
using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        SetFormWidth();
        var responsiveImageSettingsSection = ResponsiveImageSettingsSectionFactory.Create();
        FormLayout.Children.Add(responsiveImageSettingsSection.SectionLayout);
    }

    private void SetFormWidth()
    {
        FormLayout.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        FormLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
    }
}

