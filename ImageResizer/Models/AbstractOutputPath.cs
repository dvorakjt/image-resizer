namespace ImageResizer.Models;

public abstract class AbstractOutputPath
{
    public abstract string ToAbsoluteDirPathString(string ext);
    public abstract string ToRelativeDirPathString(string ext);
    public abstract string ToAbsoluteFilePathString(int width, string ext);
    public abstract string ToRelativeFilePathString(int width, string ext);
}