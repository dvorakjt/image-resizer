using NetVips;
using System.ComponentModel;
using ImageResizer.DataModel;
using Image = Microsoft.Maui.Controls.Image;

namespace ImageResizer.FormControls;

// note that the image and container sizes are not set but should be
// also the color of the border should be changed and reset / display errors need to be implemented
// also, this should emit a loading event

public partial class ImagePicker : ContentView, IFormElement<Stream?>, INotifyPropertyChanged
{
    public event EventHandler<IFormElementState<Stream?>> StateChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public IFormElementState<Stream?> State
    {
        get;
        private set
        {
            field = value;
            Thumbnail.Source = ImageSource.FromStream(() => field.Value);
            StateChanged?.Invoke(this, field);
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

    public void DisplayErrors()
    {
        throw new NotImplementedException();
    }
    
    public void Revalidate()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    private async void OnTapped(object sender, EventArgs e)
    {
        try
        {
            var pickOptions = new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.Default.PickAsync(pickOptions);
            if (result == null)
            {
                DisplayErrors();
                return;
            }

            var imageStream = await result.OpenReadAsync();

            State = new FormElementState<Stream?>
            {
                Value = imageStream,
                IsValid = true,
                ErrorMessage = ""
            };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            DisplayErrors();
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

        if(imageStream != null)
        {
            State = new FormElementState<Stream?>
            {
                Value = imageStream,
                IsValid = true,
                ErrorMessage = ""
            };
        }

        DisplayErrors();
    }

    private partial Task<bool> CanDrop(DragEventArgs e);
    
    private partial Task<Stream?> GetDroppedImageStream(DropEventArgs e);
}