using ImageResizer.Models;

namespace TestImageResizer.Models;

public class TestOutputPath
{
    private readonly string PATH_TO_PUBLIC_DIR = "/users/User/Projects/MyProject/public".Replace('/', Path.DirectorySeparatorChar);
    private string  PATH_FROM_PUBLIC_DIR = "images/pages/home".Replace('/', Path.DirectorySeparatorChar);
    private const string FILENAME = "hero";
    private const int VERSION = 1;
    private const int WIDTH = 100;
    private const string EXTENSION = ".jpg";
    
    
    [Fact]
    public void TestOutputPathToAbsoluteDirPath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualAbsoluteDirPath = outputPath.ToAbsoluteDirPathString(EXTENSION);
        var expectedAbsoluteDirPath = Path.Join(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, EXTENSION.Substring(EXTENSION.IndexOf('.') + 1));
        Assert.Equal(expectedAbsoluteDirPath, actualAbsoluteDirPath);
    }
    
    [Fact]
    public void TestOutputPathToRelativeDirPath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualRelativeDirPath = outputPath.ToRelativeDirPathString(EXTENSION);
        var expectedRelativeDirPath = Path.Join(PATH_FROM_PUBLIC_DIR, FILENAME, EXTENSION.Substring(EXTENSION.IndexOf('.') + 1));
        Assert.Equal(expectedRelativeDirPath, actualRelativeDirPath);
    }
    
    [Fact]
    public void TestOutputPathToAbsoluteFilePath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualAbsoluteFilePath = outputPath.ToAbsoluteFilePathString(WIDTH, EXTENSION);
        var expectedAbsoluteFilePath =
            Path.Join(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, EXTENSION.Substring(EXTENSION.IndexOf('.') + 1), $"{FILENAME}_{WIDTH}w_v{VERSION}{EXTENSION}");
        Assert.Equal(expectedAbsoluteFilePath, actualAbsoluteFilePath);
    }
    
    [Fact]
    public void TestOutputPathToRelativeFilePath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualRelativeFilePath = outputPath.ToRelativeFilePathString(WIDTH, EXTENSION);
        var expectedRelativeFilePath =
            Path.Join(PATH_FROM_PUBLIC_DIR, FILENAME, EXTENSION.Substring(EXTENSION.IndexOf('.') + 1), $"{FILENAME}_{WIDTH}w_v{VERSION}{EXTENSION}");
        Assert.Equal(expectedRelativeFilePath, actualRelativeFilePath);
    }
}