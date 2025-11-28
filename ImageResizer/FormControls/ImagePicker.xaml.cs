using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

public partial class ImagePicker : ContentView, IFormElement<Stream?>
{
    public event EventHandler<IFormElementState<Stream?>> StateChanged;
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
            TheThumbnail.Source = ImageSource.FromStream(() =>
            {
                /* 
                    Create a new MemoryStream and copy the image stream to allow reading 
                    from the original stream later (prevents "Cannot read from closed 
                    stream" errors).
                */
                var memoryStream = new MemoryStream();
                imageStream.CopyTo(memoryStream);

                /* 
                    Reset the positions of both streams which will both be at the end 
                    after the copy operation.
                */
                memoryStream.Position = 0;
                imageStream.Position = 0;
                return memoryStream;
            });
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