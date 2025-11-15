namespace ImageResizer.Components;

public partial class ImagePicker : ContentView
{
    public event EventHandler? BeginLoading;
    public event EventHandler? EndLoading;
    
    private Stream? _imageStream;

    public Stream? ImageStream
    {
        get => _imageStream;
        set
        {
            _imageStream = value;
            Thumbnail.Source = ImageSource.FromStream(() => _imageStream);
            
            var isValid = _imageStream != null;
            ErrorMessage.Text = isValid ? "" : "Please select an image.";
        }
    }
    
    public ImagePicker()
    {
        InitializeComponent();
    }
    
    private async void OnTapped(object sender, EventArgs e)
    {
        BeginLoading?.Invoke(this, EventArgs.Empty);
        
        try
        {
            var pickOptions = new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.Default.PickAsync(pickOptions);
            if (result == null) return;

            var imageStream = await result.OpenReadAsync();
            ImageStream = imageStream;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
        }
        finally
        {
            EndLoading?.Invoke(this, EventArgs.Empty);
        }
    }

    private async void OnDragOver(object sender, DragEventArgs e)
    {
        bool canDrop = await CanDrop(e);
        e.AcceptedOperation = canDrop ? DataPackageOperation.Copy : DataPackageOperation.None;
    }

    private async void OnDrop(object sender, DropEventArgs e)
    {
        var imageStream = await GetDroppedImageStream(e);
        ImageStream = imageStream;
    }

    private partial Task<bool> CanDrop(DragEventArgs e);
    
    private partial Task<Stream?> GetDroppedImageStream(DropEventArgs e);
}