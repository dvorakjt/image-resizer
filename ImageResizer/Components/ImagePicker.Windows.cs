namespace ImageResizer.Components;

public partial class ImagePicker
{
    private partial bool CanDrop(DragEventArgs e)
    {
        var WindowsDragEventArgs = e.PlatformArgs.DragEventArgs;
        var dragUI = WindowsDragEventArgs.DragUIOverride;

        var DraggedOverItems = await WindowsDragEventArgs.DataView.GetStorageItemsAsync();
        e.AcceptedOperation = DataPackageOperation.None;

        if (DraggedOverItems.Count > 0)
        {
            foreach (var item in DraggedOverItems)
            {
                if (item is Windows.Storage.StorageFile file)
                {
                    string fileExtension = file.FileType.ToLower();
                    if(fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png") //check any other type of file you want to accept
                    {

                        dragUI.Caption = "Drop the file!";
                        dragUI.IsCaptionVisible = false;
                        dragUI.IsGlyphVisible = false;
                            
                        filePath = file.Path;
                        Debug.WriteLine($"We now have file {file.Path} dragged over!");
                    }
                    else
                    {
                        dragUI.Caption = "Invalid file type";
                        dragUI.IsCaptionVisible = true;
                        dragUI.IsGlyphVisible = false;
                            
                    }
                }
            }
        }
    }

    private partial Task<Stream?> GetDroppedImageStream(DropEventArgs e)
    {
        throw new NotImplementedException();
    }
}