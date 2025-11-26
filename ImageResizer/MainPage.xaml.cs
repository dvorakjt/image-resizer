using System.ComponentModel;
using ImageResizer.FormControls;
using ImageResizer.FormGroups.Formats;
using ImageResizer.FormGroups.Output;
using ImageResizer.FormGroups.ResponsiveImageSettings;
using ImageResizer.FormGroups.TheImage;

namespace ImageResizer;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;

    public bool IsLoading
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
        }
    } = false;

    private TheImageFormGroup _theImageFormGroup;
    private ResponsiveImageSettingsFormGroup _responsiveImageSettingsFormGroup;
    private FormatsFormGroup _formatsFormGroup;
    private OutputFormGroup _outputFormGroup;
    
    public MainPage()
    {
        InitializeComponent();
        InitializeFormGroups();
        InitializeResizeButton();
        InitializeResetButton();
    }
    
    private void InitializeFormGroups()
    {
        _theImageFormGroup = new TheImageFormGroup();
        RootLayout.Children.Add(_theImageFormGroup);
        
        _responsiveImageSettingsFormGroup = new ResponsiveImageSettingsFormGroup();
        RootLayout.Children.Add(_responsiveImageSettingsFormGroup);
        
        _formatsFormGroup = new FormatsFormGroup();
        RootLayout.Children.Add(_formatsFormGroup);

        _outputFormGroup = new OutputFormGroup();
        RootLayout.Children.Add(_outputFormGroup);
    }

    private void InitializeResizeButton()
    {
        var resizeButton = new Button()
        {
            Text = "Resize",
            StyleClass = ["LargeButton"]
        };

        resizeButton.Clicked += async (sender, args) =>
        {
            if (!IsFormValid(out View? elementToScrollTo))
            {
                _theImageFormGroup.DisplayErrors();
                _responsiveImageSettingsFormGroup.DisplayErrors();
                _formatsFormGroup.DisplayErrors();
                _outputFormGroup.DisplayErrors();

                if (elementToScrollTo != null)
                {
                    await ScrollContainer.ScrollToAsync(elementToScrollTo, 0, false);
                }
                
                await DisplayAlertAsync("Error","Failed to resize image: invalid form field(s).", "Ok");
                
                return;
            }
            
            IsLoading = true;
            await Task.Delay(2000);
            IsLoading = false;
        };
        
        RootLayout.Children.Add(resizeButton);
    }

    private void InitializeResetButton()
    {
        var resetButton = new Button()
        {
            Text = "Reset",
            StyleClass = ["LargeButton", "SecondaryButton"]
        };

        resetButton.Clicked += (sender, args) => Reset();

        RootLayout.Children.Add(resetButton);
    }

    private bool IsFormValid(out View? elementToScrollTo)
    {
        bool isValid = true;
        elementToScrollTo = null;
        
        if (!_outputFormGroup.State.IsValid)
        {
            isValid = false;
            elementToScrollTo = _outputFormGroup;
        }
        
        if (!_formatsFormGroup.State.IsValid)
        {
            isValid = false;
            elementToScrollTo = _formatsFormGroup;
        }
        
        if (!_responsiveImageSettingsFormGroup.State.IsValid)
        {
            isValid = false;
            elementToScrollTo = _responsiveImageSettingsFormGroup;
        }
        
        if (!_theImageFormGroup.State.IsValid)
        {
            isValid = false;
            elementToScrollTo = _theImageFormGroup;
        }
        
        return isValid;
    }

    private async Task Reset()
    {
        var shouldReset = await DisplayAlert("Confirm", "Are you sure you would like to reset the form?", "Reset", "Cancel");
        
        if (shouldReset)
        {
            _theImageFormGroup.Reset();
            _responsiveImageSettingsFormGroup.Reset();
            _formatsFormGroup.Reset();
            _outputFormGroup.Reset();
        }

        await ScrollContainer.ScrollToAsync(0.0d, 0.0d, false);
    }
}