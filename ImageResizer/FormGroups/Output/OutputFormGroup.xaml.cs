using System.Text.RegularExpressions;
using ImageResizer.DataModel;
using ImageResizer.DataModel.Output;
using ImageResizer.FormControls;

namespace ImageResizer.FormGroups.Output;

public partial class OutputFormGroup : ContentView, IFormElement<OutputFormGroupValue>
{ 
    public event EventHandler<IFormElementState<OutputFormGroupValue>>? StateChanged;

    public IFormElementState<OutputFormGroupValue> State
    {
        get
        {
            var isValid = 
                _filenameInput.State.IsValid && 
                _versionIdInput.State.IsValid &&
                _pathToPublicDirInput.State.IsValid && 
                _pathFromPublicDirInput.State.IsValid;

            return new FormElementState<OutputFormGroupValue>()
            {
                Value = new OutputFormGroupValue()
                {
                    Filename = _filenameInput.State.Value,
                    VersionId = _versionIdInput.State.Value,
                    PathToPublicDirectory = _pathToPublicDirInput.State.Value,
                    PathFromPublicDirectory = _pathFromPublicDirInput.State.Value,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
    private TextInput _filenameInput;
    private TextInput _versionIdInput;
    private TextInput _pathToPublicDirInput;
    private TextInput _pathFromPublicDirInput;
    
    public OutputFormGroup()
    {
        InitializeComponent();
        InitializeFormControls();
    }

    public void Revalidate()
    {
        _filenameInput.Revalidate();
        _versionIdInput.Revalidate();
        _pathToPublicDirInput.Revalidate();
        _pathFromPublicDirInput.Revalidate();
    }

    public void DisplayErrors()
    {
        _filenameInput.DisplayErrors();
        _versionIdInput.DisplayErrors();
        _pathToPublicDirInput.DisplayErrors();
        _pathFromPublicDirInput.DisplayErrors();
    }

    public void Reset()
    {
        _filenameInput.Reset();
        _versionIdInput.Reset();
        _pathToPublicDirInput.Reset();
        _pathFromPublicDirInput.Reset();
    }

    private void InitializeFormControls()
    {
        _filenameInput = new TextInputBuilder()
            .WithLabel("File Name (without extension)")
            .WithValidator(IsValidFileName)
            .Build();

        _filenameInput.HorizontalOptions = LayoutOptions.Fill;
        _filenameInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_filenameInput);
        
        _versionIdInput = new TextInputBuilder()
            .WithLabel("Version ID (for cache busting)")
            .WithValidator(IsValidVersionId).Build();
        
        _versionIdInput.HorizontalOptions = LayoutOptions.Fill;
        _versionIdInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_versionIdInput);
        
        _pathToPublicDirInput = new TextInputBuilder()
            .WithLabel("Absolute path to the public directory of your project")
            .WithValidator(IsValidAbsolutePath)
            .Build();
        
        _pathToPublicDirInput.HorizontalOptions = LayoutOptions.Fill;
        _pathToPublicDirInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_pathToPublicDirInput);
        
        _pathFromPublicDirInput = new TextInputBuilder()
            .WithLabel("Relative path from the public directory to the output directory")
            .WithValidator(IsValidPath)
            .Build();
        
        _pathFromPublicDirInput.HorizontalOptions = LayoutOptions.Fill;
        _pathFromPublicDirInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_pathFromPublicDirInput);
    }

    private ValidatorResult IsValidFileName(string value)
    {
        // Filename is required
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Filename is required"
            };
        }
        
        // Filename cannot include / or \
        if (value.Contains('/') || value.Contains('\\'))
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Filename cannot contain potential directory separaters (/ or \\)"
            };
        }

        // Check for any other invalid characters
        if (value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = $"Filename {value} contains invalid filename characters"
            };
        }

        return new ValidatorResult
        {
            IsValid = true,
            ErrorMessage = ""
        };
    }

    private ValidatorResult IsValidVersionId(string value)
    {
        /*
            Allow for numeric, UUID, and dot-separated version ids. 
        */
        bool isValid = new Regex(@"^[a-zA-Z0-9\.\-]+$").IsMatch(value);
        return new ValidatorResult
        {
            IsValid = isValid,
            ErrorMessage = isValid ? "" : "Please enter a valid version id. Allowed characters: A-Z a-z 0-9 . -"
        };
    }
    
    private ValidatorResult IsValidAbsolutePath(string value)
    {
        bool isValid = Path.IsPathFullyQualified(value);
        return new ValidatorResult
        {
            IsValid = isValid,
            ErrorMessage = isValid ? "" : "Please enter a valid absolute path."
        };
    }

    private ValidatorResult IsValidPath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ValidatorResult
            {
                IsValid = false, 
                ErrorMessage = "Path is required"
            };
        }
        
        /*
            Accessing the parent directory of the path could cause problems when serving the image from the public 
            directory of a web project.
        */ 
        if (value.IndexOf(".." + Path.DirectorySeparatorChar) != -1)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Path cannot contain .." + Path.DirectorySeparatorChar
            };
        }

        /*
            / should be used instead of constructs like /./././
        */
        if (value.IndexOf("." + Path.DirectorySeparatorChar) > 0)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Path cannot contain ." + Path.DirectorySeparatorChar + " except at the start"
            };
        }

        /*
            When producing a URI from the path, the directory separator character will be converted to a forward slash. 
            If the path includes both backslashes and forward slashes, and one type is interpreted as a literal character 
            when the image file is written, this could potentially cause issues when serving the file, so it is best 
            to allow only one type of separator.
        */
        if (Path.DirectorySeparatorChar == '\\' && value.Contains('/'))
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Path cannot contain / if system directory separator is \\"
            };
        }
        
        if (Path.DirectorySeparatorChar == '/' && value.Contains('\\'))
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Path cannot contain \\ if system directory separator is /"
            };
        }

        // Check for any other invalid characters
        if (value.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = $"Path {value} contains invalid character"
            };
        }

        return new ValidatorResult
        {
            IsValid = true,
            ErrorMessage = ""
        };
    }
}