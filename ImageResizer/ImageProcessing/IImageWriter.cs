using ImageResizer.DataModel.Formats;
using ImageResizer.DataModel.TheImage;

namespace ImageResizer.ImageProcessing;

public interface IImageWriter
{
    Task ResizeReformatAndSaveAsAVIF(
        byte[] imageData,
        HashSet<int> widths,
        IImagePath outputPath,
        int quality,
        int effort
    );
    
    Task ResizeReformatAndSaveAsWebP(
        byte[] imageData,
        HashSet<int> widths,
        IImagePath outputPath,
        int quality,
        int effort
    );
    
    Task ResizeReformatAndSaveAsJPEG(
        byte[] imageData,
        HashSet<int> widths,
        IImagePath outputPath,
        int quality
    );
}