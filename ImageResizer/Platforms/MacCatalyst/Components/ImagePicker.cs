using UniformTypeIdentifiers;

namespace ImageResizer.Components;

public partial class ImagePicker
{
    private partial bool CanDrop(DragEventArgs e)
    {
        var dropSession = e.PlatformArgs?.DropSession;
        var items = dropSession?.Items;
        if (items != null && items.Length == 1)
        {
            var isImage = items[0].ItemProvider.CanLoadObject(typeof(UIKit.UIImage));
            return isImage;
        }
        
        return false;
    }
}
