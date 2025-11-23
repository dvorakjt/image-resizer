using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

public partial class ImagePicker : ContentView, IFormElement<Stream?>
{
    public event EventHandler<IFormElementState<Stream?>> StateChanged;
    public event EventHandler ImageLoading;
    public event EventHandler ImageLoaded;
    public event EventHandler<Exception> Error;

    private bool _shouldDisplayErrors;

    public IFormElementState<Stream?> State
    {
        get;
        private set
        {
            field = value;
            StateChanged?.Invoke(this, field);
            UpdateImageSourceAndLabel(field.Value);
            ApplyValidityDependentStyles();            
        }
    } = new FormElementState<Stream?>
    {
        Value = null,
        IsValid = false,
        ErrorMessage = ""
    };

    public ImagePicker()
    {
        InitializeComponent();
    }

    private async void OnTapped(object sender, EventArgs e)
    {
        ImageLoading?.Invoke(this, EventArgs.Empty);
        _shouldDisplayErrors = true;

        try
        {
            var pickOptions = new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
            };

            var result = await FilePicker.Default.PickAsync(pickOptions);
            
            if (result != null)
            {
                var imageStream = await result.OpenReadAsync();

                State = new FormElementState<Stream?>
                {
                    Value = imageStream,
                    IsValid = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                DisplayErrors();
            }
            
            ImageLoaded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Error?.Invoke(this, ex);
        }
    }
    
    public void DisplayErrors()
    {
        _shouldDisplayErrors = true;
        ApplyValidityDependentStyles();
    }
    
    public void Revalidate()
    {
        ;
    }

    public void Reset()
    {
        _shouldDisplayErrors = false;
        State = new FormElementState<Stream?>()
        {
            Value = null,
            IsValid = false,
            ErrorMessage = ""
        };
    }

    private async void OnDragOver(object sender, DragEventArgs e)
    {
        try
        {
            var canDrop = await CanDrop(e);
            e.AcceptedOperation = canDrop ? DataPackageOperation.Copy : DataPackageOperation.None;
        }
        catch (Exception ex)
        {
            e.AcceptedOperation = DataPackageOperation.None;
            Console.Error.WriteLine(ex);
        }
    }

    private async void OnDrop(object sender, DropEventArgs e)
    {
        ImageLoading?.Invoke(this, EventArgs.Empty);

        try
        {
            var imageStream = await GetDroppedImageStream(e);

            if (imageStream != null)
            {
                State = new FormElementState<Stream?>
                {
                    Value = imageStream,
                    IsValid = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                DisplayErrors();
            }
            
            ImageLoaded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Error?.Invoke(this, ex);
        }
    }

    private void UpdateImageSourceAndLabel(Stream? imageStream)
    {
        if (imageStream != null)
        {
            TheThumbnail.Source = ImageSource.FromStream(() => imageStream);
            TheLabel.IsVisible = false;
            TheThumbnail.IsVisible = true;
        }
        else
        {
                
            TheThumbnail.Source = null;
            TheThumbnail.IsVisible = false;
            TheLabel.IsVisible = true;
        }
    }
    
    private void ApplyValidityDependentStyles()
    {
        if (_shouldDisplayErrors && !State.IsValid)
        {
            TheBorder.Stroke = Color.Parse("Red");
            TheLabel.TextColor = Color.Parse("Red");
        }
        else
        {
            TheBorder.Stroke = Color.Parse("Black");
            TheLabel.TextColor  = Color.Parse("Black");
        }
    }

    private partial Task<bool> CanDrop(DragEventArgs e);
    
    private partial Task<Stream?> GetDroppedImageStream(DropEventArgs e);
}