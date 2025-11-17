using NetVips;
using System.ComponentModel;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public partial class ImagePicker : ContentView, IFormElement<Stream?>, IFormElementWithErrorDisplay, INotifyPropertyChanged
{
    public event EventHandler<FormElementStateChangedEventArgs<Stream?>> StateChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public FormElementState<Stream?> State
    {
        get;
        private set
        {
            field = value;
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<Stream?>(field));
            Thumbnail.Source = ImageSource.FromStream(() => field.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorMessageText)));
        }
    } = new FormElementState<Stream?>
    {
        Value = null,
        IsValid = false,
        ErrorMessage = "Please select an image."
    };

    public string ErrorMessageText { get => State.ErrorMessage ?? ""; }
    public bool IsErrorMessageVisible
    {
        get;
        private set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsErrorMessageVisible)));
        }
    }

    public ImagePicker()
    {
        InitializeComponent();
        SetImageContainerSize();
    }

    public void RevealErrors()
    {
        IsErrorMessageVisible = true;
    }

    private void SetImageContainerSize()
    {
        ImageContainer.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        ImageContainer.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
        ImageContainer.MinimumHeightRequest = AppDimensions.CONTENT_WIDTH;
        ImageContainer.MaximumHeightRequest = AppDimensions.CONTENT_WIDTH;
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
                RevealErrors();
                return;
            }

            var imageStream = await result.OpenReadAsync();

            State = new FormElementState<Stream?>
            {
                Value = imageStream,
                IsValid = true
            };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            RevealErrors();
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
                IsValid = true
            };
        }

        RevealErrors();
    }

    private partial Task<bool> CanDrop(DragEventArgs e);
    
    private partial Task<Stream?> GetDroppedImageStream(DropEventArgs e);

}