using ImageResizer.ViewFactories;
using ImageResizer.ViewModels;
using ImageResizer.Views;
using RadioButtonGroup = ImageResizer.Views.RadioButtonGroup;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
        SetFormWidth();
        var imageSection = ImageSectionFactory.CreateImageSection();
        FormLayout.Children.Add(imageSection.SectionLayout);
        var formatsSection = FormatsSectionFactory.CreateFormatsSection();
        FormLayout.Children.Add(formatsSection.SectionLayout);
    }

    private void SetFormWidth()
    {
        FormLayout.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        FormLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
    }
}

