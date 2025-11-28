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
    
    public string GetPlatformSpecificDirPath(ImageFileFormats format)
    {
        var relativePath = ConstructRelativePath(format);
        return Path.Combine(_pathToPublicDirectory, relativePath);
    }

    public string GetPlatformSpecificFilePath(ImageFileFormats format, int imageWidth)
    {
        var absolutePath = GetPlatformSpecificDirPath(format);
        var filename = ConstructFileName(format, imageWidth);
        return Path.Combine(absolutePath, filename);
    }

    public string GetURI(ImageFileFormats format, int imageWidth)
    {
        // Create a path from the public directory to the file
        var relativePath = ConstructRelativePath(format);
        var filename = ConstructFileName(format, imageWidth);
        var path = Path.Combine(relativePath, filename);
        
        // Replace the platform-specific directory separator with forward slashes
        path = path.Replace(Path.DirectorySeparatorChar, '/');
        
        // Replace leading ./ with /
        if(path.StartsWith("./")) path = path.Substring(1);
        
        // The path should begin with / if it does not already 
        if(!path.StartsWith('/')) path = '/' + path;
        
        // URI-encode the path
        var uri = Uri.EscapeDataString(path);
        return uri;
    }
    
    /// <summary>
    /// Constructs a path in the format /<pathFromPublicDir>/filename/extension
    /// </summary>
    private string ConstructRelativePath(
        ImageFileFormats format
    )
    {
        return Path.Combine(_pathFromPublicDirectory, _baseFilename, format.ToFileExtension());
    }

    /// <summary>
    /// Constructs a filename in the format <filename>_<imageWidth>w_v<versionId>.<extension>
    /// </summary>
    private string ConstructFileName(ImageFileFormats format, int imageWidth)
    {
        return $"{_baseFilename}_{imageWidth}w_v{_versionId}.{format.ToFileExtension()}";
    }
}