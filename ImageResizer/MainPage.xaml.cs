using ImageResizer.ViewFactories;
using ImageResizer.ViewModels;
using ImageResizer.Views;

namespace ImageResizer;

public partial class MainPage : ContentPage
{
    private ImageSection _imageSection;
    private FormatsSection _formatsSection;
    private ResponsiveImageSettingsSection _responsiveImageSettingsSection;
    private OutputSection _outputSection;
    
    public MainPage()
    {
        InitializeComponent();
        CreateFormElements();
    }

    private void CreateFormElements()
    {
        var leftPane = CreatePane();
        
        _imageSection = ImageSectionFactory.Create();
        leftPane.Children.Add(_imageSection.SectionLayout);
        
        _formatsSection = FormatsSectionFactory.Create();
        leftPane.Children.Add(_formatsSection.SectionLayout);
        
        FormLayout.Children.Add(leftPane);
        
        var rightPane = CreatePane();
        
        _responsiveImageSettingsSection = ResponsiveImageSettingsSectionFactory.Create();
        rightPane.Children.Add(_responsiveImageSettingsSection.SectionLayout);
        
        _outputSection = OutputSectionFactory.Create();
        rightPane.Children.Add(_outputSection.SectionLayout);
        
        var submitButton = CreateSubmitButton();
        rightPane.Children.Add(submitButton);
        
        FormLayout.Children.Add(rightPane);
    }
    
    private Layout CreatePane()
    {
        var padding = 12;
        var paneWidth = AppDimensions.CONTENT_WIDTH + padding * 2;
        var pane = new VerticalStackLayout()
        {
            Padding = new Thickness(12),
            MinimumWidthRequest = paneWidth,
            MaximumWidthRequest = paneWidth
        };
        return pane;
    }

    private Button CreateSubmitButton()
    {
        return new Button()
        {
            Text = "Resize",
            StyleClass = ["Submit"]
        };
    }
}

