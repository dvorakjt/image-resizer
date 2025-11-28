using ImageResizer.DataModel.Formats;

namespace ImageResizer.ImageProcessing;

public interface IImagePath
{
    string GetPlatformSpecificDirPath(ImageFileFormats format);
    string GetPlatformSpecificFilePath(ImageFileFormats format, int imageWidth);
    string GetURI(ImageFileFormats format, int imageWidth);
}