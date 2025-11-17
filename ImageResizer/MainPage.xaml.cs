using ImageResizer.Models;
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
        var submitButton = new Button()
        {
            Text = "Resize",
            StyleClass = ["Submit"]
        };

        submitButton.Clicked += (sender, e) => ResizeImage();
        return submitButton;
    }

    private bool ResizeImage()
    {
        bool isFormValid = ValidateForm();
        return isFormValid;
    }

    private bool ValidateForm()
    {
        bool isValid = ValidateImageSection();
        if(!ValidateFormatsSection()) isValid = false;
        if(!ValidateResponsiveImageSettingsSection()) isValid = false;
        if(!ValidateOutputSection()) isValid = false;
        return isValid;
    }

    private bool ValidateImageSection()
    {
        bool isValid = _imageSection.ImagePicker.State.IsValid;

        if (!_imageSection.AltTextInput.State.IsValid)
        {
            isValid = false;
        }
        
        _imageSection.ImagePicker.RevealErrors();
        _imageSection.AltTextInput.RevealErrors();
        return isValid;
    }

    private bool ValidateFormatsSection()
    {
        bool isValid = !_formatsSection.SelectedFormats.State.Value.Contains(OutputFormat.AVIF.ToFileExtension()) ||
                       _formatsSection.AVIFOptionsInput.State.IsValid;
        
        if 
        (
            _formatsSection.SelectedFormats.State.Value.Contains(OutputFormat.WebP.ToFileExtension()) &&
            !_formatsSection.WebPOptionsInput.State.IsValid
        )
        {
            isValid = false;
            
        }

        if (!_formatsSection.JPGQualityInput.State.IsValid)
        {
            isValid = false;
        }
        
        _formatsSection.AVIFOptionsInput.RevealErrors();
        _formatsSection.WebPOptionsInput.RevealErrors();
        _formatsSection.JPGQualityInput.RevealErrors();
        
        return isValid;
    }

    private bool ValidateResponsiveImageSettingsSection()
    {
        bool isValid = _responsiveImageSettingsSection.ResponsivenessModeInput.State.Value !=
                       ResponsivenessMode.Densities.ToString() ||
                       _responsiveImageSettingsSection.DensitiesInput.State.IsValid;

        if 
        (
            _responsiveImageSettingsSection.ResponsivenessModeInput.State.Value == ResponsivenessMode.Widths.ToString() &&
            !_responsiveImageSettingsSection.WidthsInput.State.IsValid
        )
        {
            isValid = false;
        }
        
        if 
        (
            _responsiveImageSettingsSection.ResponsivenessModeInput.State.Value == ResponsivenessMode.MediaQueries.ToString() &&
            !_responsiveImageSettingsSection.MediaQueriesInput.State.IsValid
        )
        {
            isValid = false;
        }
        
        _responsiveImageSettingsSection.DensitiesInput.RevealErrors();
        _responsiveImageSettingsSection.WidthsInput.RevealErrors();
        _responsiveImageSettingsSection.MediaQueriesInput.RevealErrors();

        return isValid;
    }

    private bool ValidateOutputSection()
    {
        bool isValid = _outputSection.FileNameInput.State.IsValid;
        if(!_outputSection.VersionNumberInput.State.IsValid) isValid = false;
        if(!_outputSection.PathFromPublicDirInput.State.IsValid) isValid = false;
        if(!_outputSection.PathToPublicDirInput.State.IsValid) isValid = false;
        
        _outputSection.FileNameInput.RevealErrors();
        _outputSection.VersionNumberInput.RevealErrors();
        _outputSection.PathToPublicDirInput.RevealErrors();
        _outputSection.PathFromPublicDirInput.RevealErrors();
        
        return isValid;
    }
}

