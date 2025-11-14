namespace ImageResizer.Components;

public partial class ImagePicker
{
    private partial bool CanDrop(DragEventArgs e)
    {
        return true;
    }

    private partial Task<Stream?> GetDroppedImageStream(DropEventArgs e)
    {
        throw new NotImplementedException();
    }
}