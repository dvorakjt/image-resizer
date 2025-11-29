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
                    PathFromPublicDirectory = FormatRelativePath(_pathFromPublicDirInput.State.Value)
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
        
        // only allow A-Z a-z 0-9 - in filename
        var filenamePattern = new Regex(@"^[A-Za-z0-9\-]+$");
        if (!filenamePattern.IsMatch(value))
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "Please enter a valid filename. Allowed characters: A-Z a-z 0-9 -"
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
            Allow for numeric and UUID-based version numbers
        */
        bool isValid = new Regex(@"^[A-Za-z0-9\-]+$").IsMatch(value);
        return new ValidatorResult
        {
            IsValid = isValid,
            ErrorMessage = isValid ? "" : "Please enter a valid version id. Allowed characters: A-Z a-z 0-9 -"
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
        // Does not allow spaces, most symbols, combining forward slashes or backslashes, or repeated slashes
        var pathPattern = new Regex(@"^(?:(?:(?:\/|\.\/)?(?:[A-Za-z0-9\-_]+(?:\/)?)*)|(?:(?:\\|\.\\)?(?:[A-Za-z0-9\-_]+(?:\\)?)*))$");
        var isValid = pathPattern.IsMatch(value);
        
        return new ValidatorResult
        {
            IsValid = isValid,
            ErrorMessage = isValid ? "" : "Please enter a valid path. Allowed characters: A-Z a-z 0-9 - _ and / or \\"
        };
    }

    private string FormatRelativePath(string relativePath)
    {
        var formattedPath = Regex.Replace(relativePath, @"[\\\/]", Path.DirectorySeparatorChar.ToString());
        if(formattedPath.StartsWith(Path.DirectorySeparatorChar.ToString())) formattedPath = formattedPath.Substring(1);
        return formattedPath;
    }
}