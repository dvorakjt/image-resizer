using ImageResizer.DataModel;
using ImageResizer.DataModel.Formats;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Formats;

public partial class FormatsFormGroup : ContentView, IFormElement<FormatsFormGroupValue>
{
    public event EventHandler<IFormElementState<FormatsFormGroupValue>>? StateChanged;

    public IFormElementState<FormatsFormGroupValue> State
    {
        get
        {
            var isValid = _selectedOutputFormats.State.IsValid;
            var selectedFormats = _selectedOutputFormats.State.Value.Select(format =>
            {
                if (format == ImageFileFormats.AVIF.ToString())
                {
                    return ImageFileFormats.AVIF;
                }

                if (format == ImageFileFormats.WebP.ToString())
                {
                    return ImageFileFormats.WebP;
                }

                if (format == ImageFileFormats.JPEG.ToString())
                {
                    return ImageFileFormats.JPEG;
                }

                throw new InvalidOperationException($"Unsupported format: {format}");
            });

            if (selectedFormats.Contains(ImageFileFormats.AVIF))
            {
                if (!_avifOptionsFormGroup.State.IsValid)
                {
                    isValid = false;
                }
            }
            
            if (selectedFormats.Contains(ImageFileFormats.WebP))
            {
                if (!_webpOptionsFormGroup.State.IsValid)
                {
                    isValid = false;
                }
            }
            
            if (selectedFormats.Contains(ImageFileFormats.JPEG))
            {
                if (!_jpegOptionsFormGroup.State.IsValid)
                {
                    isValid = false;
                }
            }

            return new FormElementState<FormatsFormGroupValue>
            {
                Value = new FormatsFormGroupValue
                {
                    SelectedFormats = selectedFormats,
                    AVIFOptions = _avifOptionsFormGroup.State.Value,
                    WebPOptions = _webpOptionsFormGroup.State.Value,
                    JPEGOptions = _jpegOptionsFormGroup.State.Value,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
    CustomCheckboxGroup _selectedOutputFormats;
    AVIFOptionsFormGroup _avifOptionsFormGroup;
    WebPOptionsFormGroup _webpOptionsFormGroup;
    JPEGOptionsFormGroup _jpegOptionsFormGroup;
    
    public FormatsFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    public void DisplayErrors()
    {
        _selectedOutputFormats.DisplayErrors();
        _avifOptionsFormGroup.DisplayErrors();
        _webpOptionsFormGroup.DisplayErrors();
        _jpegOptionsFormGroup.DisplayErrors();
    }
    
    public void Revalidate()
    {
        _selectedOutputFormats.Revalidate();
        _avifOptionsFormGroup.Revalidate();
        _webpOptionsFormGroup.Revalidate();
        _jpegOptionsFormGroup.Revalidate();
    }
    
    public void Reset()
    {
        _selectedOutputFormats.Reset();
        _avifOptionsFormGroup.Reset();
        _webpOptionsFormGroup.Reset();
        _jpegOptionsFormGroup.Reset();
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

        _selectedOutputFormats.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
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
        _avifOptionsFormGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
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
        _webpOptionsFormGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
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
        _jpegOptionsFormGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        _selectedOutputFormats.StateChanged += (sender, e) =>
        {
            _jpegOptionsFormGroup.IsVisible = e.Value.Contains(ImageFileFormats.JPEG.ToString());
        };
        
        formatOptionsLayout.Children.Add(_jpegOptionsFormGroup);
    }
}

