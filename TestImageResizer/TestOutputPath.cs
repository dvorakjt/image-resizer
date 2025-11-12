using ImageResizer.Models;

namespace TestImageResizer;

public class TestOutputPath
{
    private const string PATH_TO_PUBLIC_DIR = "/users/User/Projects/MyProject/public";
    private const string PATH_FROM_PUBLIC_DIR = "images/pages/home";
    private const string FILENAME = "hero";
    private const int VERSION = 1;
    private const int WIDTH = 100;
    private const string EXTENSION = ".jpg";
    
    
    [Fact]
    public void TestOutputPathToAbsoluteDirPath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualAbsoluteDirPath = outputPath.ToAbsoluteDirPathString(EXTENSION);
        var expectedAbsoluteDirPath = $"{PATH_TO_PUBLIC_DIR}/{PATH_FROM_PUBLIC_DIR}/{FILENAME}/{EXTENSION.Substring(EXTENSION.IndexOf('.') + 1)}";
        Assert.Equal(expectedAbsoluteDirPath, actualAbsoluteDirPath);
    }
    
    [Fact]
    public void TestOutputPathToRelativeDirPath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualRelativeDirPath = outputPath.ToRelativeDirPathString(EXTENSION);
        var expectedRelativeDirPath = $"{PATH_FROM_PUBLIC_DIR}/{FILENAME}/{EXTENSION.Substring(EXTENSION.IndexOf('.') + 1)}";
        Assert.Equal(expectedRelativeDirPath, actualRelativeDirPath);
    }
    
    [Fact]
    public void TestOutputPathToAbsoluteFilePath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualAbsoluteFilePath = outputPath.ToAbsoluteFilePathString(WIDTH, EXTENSION);
        var expectedAbsoluteFilePath =
            $"{PATH_TO_PUBLIC_DIR}/{PATH_FROM_PUBLIC_DIR}/{FILENAME}/{EXTENSION.Substring(EXTENSION.IndexOf('.') + 1)}/{FILENAME}_{WIDTH}w_v{VERSION}{EXTENSION}";
        Assert.Equal(expectedAbsoluteFilePath, actualAbsoluteFilePath);
    }
    
    [Fact]
    public void TestOutputPathToRelativeFilePath()
    {
        var outputPath = new OutputPath(PATH_TO_PUBLIC_DIR, PATH_FROM_PUBLIC_DIR, FILENAME, VERSION);
        var actualRelativeFilePath = outputPath.ToRelativeFilePathString(WIDTH, EXTENSION);
        var expectedRelativeFilePath =
            $"{PATH_FROM_PUBLIC_DIR}/{FILENAME}/{EXTENSION.Substring(EXTENSION.IndexOf('.') + 1)}/{FILENAME}_{WIDTH}w_v{VERSION}{EXTENSION}";
        Assert.Equal(expectedRelativeFilePath, actualRelativeFilePath);
    }
}