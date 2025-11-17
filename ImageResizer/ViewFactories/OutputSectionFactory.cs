using ImageResizer.ViewModels;
using ImageResizer.Views;

namespace ImageResizer.ViewFactories;

public class OutputSection
{
    public Layout SectionLayout { get; init; }
    public TextInput FileNameInput { get; init; }
    public TextInput VersionNumberInput { get; init; }
    public TextInput PathToPublicDirInput { get; init; }
    public TextInput PathFromPublicDirInput { get; init; }
}

public static class OutputSectionFactory
{
    public static OutputSection Create()
    {
        var sectionLayout = SectionLayoutFactory.Create();
        var heading = CreateHeading();
        sectionLayout.Children.Add(heading);
        
        var fileNameInput = CreateFileNameInput();
        sectionLayout.Children.Add(fileNameInput);
        
        var versionNumberInput = CreateVersionNumberInput();
        sectionLayout.Children.Add(versionNumberInput);
        
        var pathToPublicDirInput = CreatePathToPublicDirInput();
        sectionLayout.Children.Add(pathToPublicDirInput);
        
        var pathFromPublicDirInput = CreatePathFromPublicDirInput();
        sectionLayout.Children.Add(pathFromPublicDirInput);

        return new OutputSection()
        {
            SectionLayout = sectionLayout,
            FileNameInput = fileNameInput,
            VersionNumberInput = versionNumberInput,
            PathToPublicDirInput = pathToPublicDirInput,
            PathFromPublicDirInput = pathFromPublicDirInput
        };
    }

    private static Label CreateHeading()
    {
        var heading = new Label()
        {
            Text = "Output",
            StyleClass = ["SubHeading1"]
        };
        return heading;
    }

    private static TextInput CreateFileNameInput()
    {
        var fileNameInput = new TextInput
            (
                "",
                FormElementHelpers
                    .CreateRequiredFieldValidator("Please enter a file name.")
            )
            {
                LabelText = "File Name (without extension)",
                MaxLength = 255,
                Margin = new Thickness(0, 0, 0, 10),
            };
        
        return fileNameInput;
    }

    private static TextInput CreateVersionNumberInput()
    {
        var versionNumberInput = new TextInput
            (
                "",
                FormElementHelpers.CreateRequiredFieldValidator("Please enter a version number."),
                FormElementHelpers.AllowOnlyDigits
            )
            {
                LabelText = "Version Number (for cache busting)",
                MaxLength = 255,
                Margin = new Thickness(0, 0, 0, 10),
            };
        
        return versionNumberInput;
    }

    private static TextInput CreatePathToPublicDirInput()
    {
        ValidatorFuncResult IsAbsolutePath(string value)
        {
            bool isValid = Path.IsPathFullyQualified(value);
            return new ValidatorFuncResult(isValid, isValid ? "" : "Please enter an absolute path.");
        }

        var pathToPublicDirInput = new TextInput
            (
                "", 
                IsAbsolutePath
            )
            {
                LabelText = "Path to the public directory of your project",
                MaxLength = 255,
                Margin = new Thickness(0, 0, 0, 10),
            };
        
        return pathToPublicDirInput;
    }

    private static TextInput CreatePathFromPublicDirInput()
    {
        ValidatorFuncResult IsRelativePath(string value)
        {
            bool isValidPath = value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1;

            if (!isValidPath)
            {
                return new ValidatorFuncResult(false, "Please enter a valid path.");
            }

            bool isAbsolutePath = Path.IsPathFullyQualified(value);
            if (isAbsolutePath)
            {
                return new ValidatorFuncResult(false, "You have entered an absolute path. Please enter a relative path instead.");
            }

            return new ValidatorFuncResult(true, "");
        }

        var pathFromPublicDirInput = new TextInput("", IsRelativePath)
        {
            LabelText = "Path from the public directory to store images",
            MaxLength = 255,
        };
        
        return pathFromPublicDirInput;
    }
}