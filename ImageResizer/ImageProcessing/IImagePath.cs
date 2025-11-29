using ImageResizer.DataModel.Formats;

namespace ImageResizer.ImageProcessing;

public interface IImagePath
{
    string GetPlatformSpecificDirPath(ImageFileFormat format);
    string GetPlatformSpecificFilePath(ImageFileFormat format, int imageWidth);
    string GetURI(ImageFileFormat format, int imageWidth);
}