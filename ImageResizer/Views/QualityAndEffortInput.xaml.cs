using System.ComponentModel;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public struct QualityAndEffort
{
    public int? Quality;
    public int? Effort;
}

public partial class QualityAndEffortInput : ContentView, IFormElement<QualityAndEffort>, IFormElementWithErrorDisplay, INotifyPropertyChanged
{
    public static BindableProperty LabelTextProperty =
        BindableProperty.Create("LabelText", typeof(string), typeof(TextInput), "");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set {
            SetValue(LabelTextProperty, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<FormElementStateChangedEventArgs<QualityAndEffort>>? StateChanged;
    
    public FormElementState<QualityAndEffort> State
    {
        get
        {
            int? quality = null;
            
            if (_qualityInput.State.IsValid && int.TryParse(_qualityInput.State.Value, out int parsedQuality))
            {
                quality = parsedQuality;
            }
            
            int? effort = null;
            
            if (_effortInput.State.IsValid && int.TryParse(_effortInput.State.Value, out int parsedEffort))
            {
                effort = parsedEffort;
            }

            var qualityAndEffort = new QualityAndEffort()
            {
                Quality = quality,
                Effort = effort
            };

            var isValid = _qualityInput.State.IsValid && _effortInput.State.IsValid;

            return new FormElementState<QualityAndEffort>()
            {
                Value = qualityAndEffort,
                IsValid = isValid,
            };
        }
    }

    private readonly int _defaultQuality;
    private readonly int _defaultEffort;
    private TextInput _qualityInput;
    private TextInput _effortInput;
    
    public QualityAndEffortInput((int, int) minMaxQuality, (int, int) minMaxEffort, int defaultQuality, int defaultEffort)
    {
        InitializeComponent();
        
        _defaultQuality = defaultQuality;
        _defaultEffort = defaultEffort;
        
        InitializeInputElements(minMaxQuality, minMaxEffort);
    }

    private void InitializeInputElements((int, int) minMaxQuality, (int, int) minMaxEffort)
    {
        var (minQuality, maxQuality) = minMaxQuality;
        var (minEffort, maxEffort) = minMaxEffort;

        _qualityInput = new TextInput
        (
            _defaultQuality.ToString(),
            FormElementHelpers.CreateMinMaxValidator(minQuality, maxQuality,
                $"Please enter a value between {minQuality} and {maxQuality}."),
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Quality",
            MaxLength = maxQuality.ToString().Length,
        };

        _qualityInput.StateChanged += (sender, args) =>
        {
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<QualityAndEffort>(State));
        };
        
        MainLayout.Children.Add(_qualityInput);

        _effortInput = new TextInput
        (
            _defaultEffort.ToString(),
            FormElementHelpers.CreateMinMaxValidator(minEffort, maxEffort,
                $"Please enter a value between {minEffort} and {maxEffort}."),
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Effort",
            MaxLength = maxEffort.ToString().Length,
        };

        _effortInput.StateChanged += (sender, args) =>
        {
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<QualityAndEffort>(State));
        };
        
        MainLayout.Children.Add(_effortInput);
    }

    public void RevealErrors()
    {
        throw new NotImplementedException();
    }
}