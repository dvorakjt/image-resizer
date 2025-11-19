using ImageResizer.DataModel;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Formats;

public partial class FormatsFormGroup : ContentView
{
    
    CustomCheckboxGroup _selectedOutputFormats;
    AVIFOptionsFormGroup _avifOptionsFormGroup;
    WebPOptionsFormGroup _webpOptionsFormGroup;
    JPEGOptionsFormGroup _jpegOptionsFormGroup;
    
    public FormatsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    private void InitializeFormControls()
    {
        InitializeSelectedOutputFormats();
        InitializeFormatOptionsSection();
    }

    private void InitializeSelectedOutputFormats()
    {
        _selectedOutputFormats = new CustomCheckboxGroup([
            new CheckboxGroupItem
            {
                Label = "AVIF",
                Value = ImageFileFormats.AVIF.ToString(),
                IsChecked = true,
                IsFrozen = false
            },
            new CheckboxGroupItem
            {
                Label = "WebP",
                Value = ImageFileFormats.WebP.ToString(),
                IsChecked = true,
                IsFrozen = false
            },
            new CheckboxGroupItem
            {
                Label = "JPEG",
                Value = ImageFileFormats.JPEG.ToString(),
                IsChecked = true,
                IsFrozen = true
            }
        ])
        {
            LabelText = "Selected Output Formats",
        };
        
        RootLayout.Children.Add(_selectedOutputFormats);
    }

    private void InitializeFormatOptionsSection()
    {
        var formatOptionsSection = new VerticalStackLayout()
        {
            Spacing = 5
        };

        InitializeAVIFOptionsFormGroup(formatOptionsSection);
        InitializeWebPOptionsFormGroup(formatOptionsSection);
        InitializeJPEGOptionsFormGroup(formatOptionsSection);
        
        RootLayout.Children.Add(formatOptionsSection);
    }

    private void InitializeAVIFOptionsFormGroup(Layout formatOptionsLayout)
    {
        _avifOptionsFormGroup = new AVIFOptionsFormGroup();
        _avifOptionsFormGroup.IsVisible = _selectedOutputFormats.State.Value.Contains(ImageFileFormats.AVIF.ToString());
        _selectedOutputFormats.StateChanged += (sender, e) =>
        {
            _avifOptionsFormGroup.IsVisible = e.Value.Contains(ImageFileFormats.AVIF.ToString());
        };
        
        formatOptionsLayout.Children.Add(_avifOptionsFormGroup);
    }
    
    private void InitializeWebPOptionsFormGroup(Layout formatOptionsLayout)
    {
        _webpOptionsFormGroup = new WebPOptionsFormGroup();
        _webpOptionsFormGroup.IsVisible = _selectedOutputFormats.State.Value.Contains(ImageFileFormats.WebP.ToString());
        _selectedOutputFormats.StateChanged += (sender, e) =>
        {
            _webpOptionsFormGroup.IsVisible = e.Value.Contains(ImageFileFormats.WebP.ToString());
        };
        
        formatOptionsLayout.Children.Add(_webpOptionsFormGroup);
    }

    private void InitializeJPEGOptionsFormGroup(Layout formatOptionsLayout)
    {
        _jpegOptionsFormGroup = new JPEGOptionsFormGroup();
        _jpegOptionsFormGroup.IsVisible = _selectedOutputFormats.State.Value.Contains(ImageFileFormats.JPEG.ToString());
        _selectedOutputFormats.StateChanged += (sender, e) =>
        {
            _jpegOptionsFormGroup.IsVisible = e.Value.Contains(ImageFileFormats.JPEG.ToString());
        };
        
        formatOptionsLayout.Children.Add(_jpegOptionsFormGroup);
    }
}

