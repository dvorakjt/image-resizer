using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Components;

public partial class ImagePicker : ContentView
{
    public event EventHandler? BeginLoading;
    public event EventHandler? EndLoading;
    public event EventHandler<Stream?>? ImageChanged;
    
    private Stream? _imageStream;

    public Stream? ImageStream
    {
        get => _imageStream;
        set
        {
            _imageStream = value;
            Thumbnail.Source = ImageSource.FromStream(() => _imageStream);
            ImageChanged?.Invoke(this, value);
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
}