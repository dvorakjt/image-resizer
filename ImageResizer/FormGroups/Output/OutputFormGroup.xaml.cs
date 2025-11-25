using System.Text.RegularExpressions;
using ImageResizer.DataModel;
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
                _versionNumberInput.State.IsValid &&
                _pathToPublicDirInput.State.IsValid && 
                _pathFromPublicDirInput.State.IsValid;

            return new FormElementState<OutputFormGroupValue>()
            {
                Value = new OutputFormGroupValue()
                {
                    Filename = _filenameInput.State.Value,
                    Version = _versionNumberInput.State.Value,
                    PathToPublicDirectory = _pathToPublicDirInput.State.Value,
                    PathFromPublicDirectory = _pathFromPublicDirInput.State.Value,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }
    
    private TextInput _filenameInput;
    private TextInput _versionNumberInput;
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
        _versionNumberInput.Revalidate();
        _pathToPublicDirInput.Revalidate();
        _pathFromPublicDirInput.Revalidate();
    }

    public void DisplayErrors()
    {
        _filenameInput.DisplayErrors();
        _versionNumberInput.DisplayErrors();
        _pathToPublicDirInput.DisplayErrors();
        _pathFromPublicDirInput.DisplayErrors();
    }

    public void Reset()
    {
        _filenameInput.Reset();
        _versionNumberInput.Reset();
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
        
        _versionNumberInput = new TextInputBuilder()
            .WithLabel("Version Number (for cache busting)")
            .WithValidator(IsValidVersionNumber).Build();
        
        _versionNumberInput.HorizontalOptions = LayoutOptions.Fill;
        _versionNumberInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_versionNumberInput);
        
        _pathToPublicDirInput = new TextInputBuilder()
            .WithLabel("Absolute path to the public directory of your project")
            .WithValidator(IsValidAbsolutePath)
            .Build();
        
        _pathToPublicDirInput.HorizontalOptions = LayoutOptions.Fill;
        _pathToPublicDirInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_pathToPublicDirInput);
        
        _pathFromPublicDirInput = new TextInputBuilder()
            .WithLabel("Relative path from the public directory to the output directory")
            .WithValidator(IsValidRelativePath)
            .Build();
        
        _pathFromPublicDirInput.HorizontalOptions = LayoutOptions.Fill;
        _pathFromPublicDirInput.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        FormControlsLayout.Children.Add(_pathFromPublicDirInput);
    }

    private ValidatorResult IsValidFileName(string value)
    {
        bool isValid = new Regex(@"^(?:[a-zA-Z0-9\-_]+)(?:\.[a-zA-Z0-9\-_]+)*$").IsMatch(value);

        return new ValidatorResult
        {
            IsValid = isValid,
            ErrorMessage = isValid ? "" : "Please enter a valid file name (allowed chars: A-Z a-z 0-9 - _)"
        };
    }

    private ValidatorResult IsValidVersionNumber(string value)
    {
        bool isValid = new Regex(@"^\d+(?:\.\d+)*$").IsMatch(value);
        return new ValidatorResult
        {
            IsValid = isValid,
            ErrorMessage = isValid ? "" : "Please enter a valid version number (e.g. 0 or 1.0.2)"
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

    private ValidatorResult IsValidRelativePath(string value)
    {
        bool isValidPath = value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1;

        if (!isValidPath)
        {
            return new ValidatorResult
            {
                IsValid = false, 
                ErrorMessage = "Please enter a valid path."
                
            };
        }

        bool isAbsolutePath = Path.IsPathFullyQualified(value);
        if (isAbsolutePath)
        {
            return new ValidatorResult
            {
                IsValid = false,
                ErrorMessage = "You have entered an absolute path. Please enter a relative path instead."
            };
        }

        return new ValidatorResult
        {
            IsValid = true,
            ErrorMessage = ""
        };
    }
}