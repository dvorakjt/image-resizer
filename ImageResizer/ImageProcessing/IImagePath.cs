namespace ImageResizer.ImageProcessing;

public interface IImagePath
{
    string GetPlatformSpecificDirPath();
    string GetPlatformSpecificFilePath();
    string GetURI();
}