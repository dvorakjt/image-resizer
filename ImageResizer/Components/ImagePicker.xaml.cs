using NetVips;
using System.ComponentModel;

namespace ImageResizer.Components;

public partial class ImagePicker : ContentView, IFormElement<Stream?>, IFormElementWithErrorDisplay, INotifyPropertyChanged
{
    public event EventHandler? BeginLoading;
    public event EventHandler? EndLoading;
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
    }

    public void RevealErrors()
    {
        IsErrorMessageVisible = true;
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