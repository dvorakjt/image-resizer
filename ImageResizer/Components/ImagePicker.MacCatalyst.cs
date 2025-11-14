using AppKit;
using UIKit;
using UniformTypeIdentifiers;

namespace ImageResizer.Components;

public partial class ImagePicker
{
    private partial Task<bool> CanDrop(DragEventArgs e)
    {
        var dropSession = e.PlatformArgs?.DropSession;
        return Task.FromResult(CanDrop(dropSession));
    }
    
    private partial async Task<Stream?> GetDroppedImageStream(DropEventArgs e)
    {
        var dropSession = e.PlatformArgs?.DropSession;
        if (CanDrop(dropSession))
        {
            var imageData = await dropSession!.Items[0].ItemProvider.LoadDataRepresentationAsync("public.image");
            return imageData.AsStream();
        }

        return null;
    }

    private bool CanDrop(IUIDragDropSession? dropSession)
    {
        var items = dropSession?.Items;
        if (items != null && items.Length == 1)
        {
            var isImage = items[0].ItemProvider.HasItemConformingTo("public.image");
            return isImage;
        }
        
        return false;
    }
}