using ImageResizer.DataModel.Formats;

namespace ImageResizer.ImageProcessing;

public class ImagePath : IImagePath
{
    private string _pathToPublicDirectory;
    private string _pathFromPublicDirectory;
    private string _baseFilename;
    private string _versionId;
    
    public ImagePath(
        string pathToPublicDirectory,
        string pathFromPublicDirectory,
        string baseFilename,
        string versionId
    )
    {
        _pathToPublicDirectory = pathToPublicDirectory;
        _pathFromPublicDirectory = pathFromPublicDirectory;
        _baseFilename = baseFilename;
        _versionId = versionId;
    }
    
    public string GetPlatformSpecificDirPath(ImageFileFormat format)
    {
        var relativePath = ConstructRelativePath(format);
        return Path.Combine(_pathToPublicDirectory, relativePath);
    }

    public string GetPlatformSpecificFilePath(ImageFileFormat format, int imageWidth)
    {
        var absolutePath = GetPlatformSpecificDirPath(format);
        var filename = ConstructFileName(format, imageWidth);
        return Path.Combine(absolutePath, filename);
    }

    public string GetURI(ImageFileFormat format, int imageWidth)
    {
        var relativePath = ConstructRelativePath(format);
        var filename = ConstructFileName(format, imageWidth);
        var uri = Path.Combine(relativePath, filename);
        uri = uri.Replace(Path.DirectorySeparatorChar, '/');
        if (uri.StartsWith("./")) uri = uri.Substring(1);
        if(!uri.StartsWith("/")) uri = "/" + uri;
        return uri;
    }
    
    /// <summary>
    /// Constructs a path in the format /<pathFromPublicDir>/filename/extension
    /// </summary>
    private string ConstructRelativePath(
        ImageFileFormat format
    )
    {
        return Path.Combine(_pathFromPublicDirectory, _baseFilename, format.ToFileExtension());
    }

    /// <summary>
    /// Constructs a filename in the format <filename>_<imageWidth>w_v<versionId>.<extension>
    /// </summary>
    private string ConstructFileName(ImageFileFormat format, int imageWidth)
    {
        return $"{_baseFilename}_{imageWidth}w_v{_versionId}.{format.ToFileExtension()}";
    }
}