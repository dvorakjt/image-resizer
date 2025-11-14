namespace ImageResizer.Components;

public partial class ImagePicker
{
    private IEnumerable<string> _imageFileExtensions = new List<string>
    {
        ".apng",
        ".avif",
        ".gif",
        ".jpg",
        ".jpeg",
        ".jfif",
        ".pjpeg",
        ".pjp",
        ".png",
        ".svg",
        ".webp"
    };

    private partial async Task<bool> CanDrop(DragEventArgs e)
    {
        var items = await e.PlatformArgs?.DragEventArgs.DataView.GetStorageItemsAsync();
        if(items != null && items.Count == 1)
        {
            return IsImage(items[0].Name);
        }

        return false;
    }

    private async partial Task<Stream?> GetDroppedImageStream(DropEventArgs e)
    {
        var items = await e.PlatformArgs?.DragEventArgs.DataView.GetStorageItemsAsync();
        
        if (items != null && items.Count == 1 && IsImage(items[0].Name))
        {
            var pathToImage = items[0].Path;

            if (pathToImage != null)
            {
                return new FileStream(pathToImage, FileMode.Open, FileAccess.Read);
            }
        }

        return null;
    }

    private bool IsImage(string name)
    {
        return _imageFileExtensions.Any(ext => name.EndsWith(ext));
    }
}