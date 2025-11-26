using System.Text.RegularExpressions;
using ImageResizer.DataModel.Formats;

namespace ImageResizer.ImageProcessing;

public class ImagePath : IImagePath
{
    private string _basePath;
    private string _relativePath;
    private string _filename;

    public ImagePath(
        string pathToPublicDir,
        string pathFromPublicDir,
        string filename,
        int imageWidth,
        string versionNumber,
        ImageFileFormats format
    )
    {
        _basePath = pathToPublicDir;
        _relativePath = ConstructRelativePath(pathFromPublicDir, filename, format);
        _filename = ConstructFileName(filename, imageWidth, versionNumber, format);
    }
    
    public string GetPlatformSpecificDirPath()
    {
        return Path.Combine(_basePath, _relativePath);
    }

    public string GetPlatformSpecificFilePath()
    {
        return Path.Combine(GetPlatformSpecificDirPath(), _filename);
    }

    public string GetURI()
    {
        // Create a path from the public directory to the file
        var path = Path.Combine(_relativePath, _filename);
        
        // Replace the platform-specific directory separator with forward slashes
        path = path.Replace(Path.DirectorySeparatorChar, '/');
        
        // Replace leading ./ with /
        if(path.StartsWith("./")) path = path.Substring(1);
        
        // The path should begin with / if it does not already 
        if(!path.StartsWith('/')) path = '/' + path;
        
        // URI-encode the path
        var uri = Uri.EscapeUriString(path);
        return uri;
    }

    /// <summary>
    /// Constructs a path in the format /<pathFromPublicDir>/filename/extension
    /// </summary>
    private string ConstructRelativePath(
        string pathFromPublicDir,
        string filename,
        ImageFileFormats format
    )
    {
        return Path.Combine(pathFromPublicDir, filename, format.ToFileExtension());
    }

    /// <summary>
    /// Constructs a filename in the format <filename>_<imageWidth>w_v<versionId>.<extension>
    /// </summary>
    private string ConstructFileName(string filename, int imageWidth, string versionNumber, ImageFileFormats format)
    {
        return $"{filename}_{imageWidth}w_v{versionNumber}.{format.ToFileExtension()}";
    }
}