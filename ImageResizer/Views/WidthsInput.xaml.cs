using System.Collections.Specialized;
using System.ComponentModel;
using ImageResizer.Models;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public class WidthInput : TextInput
{
    public WidthInput() : base("",
        FormElementHelpers.CreateMinMaxValidator(1, 20_000, "Please enter a valid image width (min. 1, max. 20,000)."),
        FormElementHelpers.AllowOnlyDigits){}
}

public struct ScreenAndImageWidths : IComparable<ScreenAndImageWidths>
{
    public required int ScreenWidth { get; init; }
    public int? ImageWidth { get; init; }
    
    public int CompareTo(ScreenAndImageWidths other)
    {
        return this.ScreenWidth.CompareTo(other.ScreenWidth);
    }
}

public struct WidthsInputValue
{
    public required WidthComparisonMode  WidthComparisonMode { get; init; }
    public int? DefaultWidth { get; init; }
    public required IEnumerable<ScreenAndImageWidths> ScreenAndImageWidthsList { get; init; }
}

public partial class WidthsInput : ContentView, IFormElement<WidthsInputValue>, IFormElementWithErrorDisplay, INotifyCollectionChanged
{ 
    public event EventHandler<FormElementStateChangedEventArgs<WidthsInputValue>>? StateChanged;
    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public FormElementState<WidthsInputValue> State
    {
        get
        {
            var canParseDefaultWidth = int.TryParse(_defaultImageWidthInput.State.Value, out var defaultWidth);
            var isValid = true; //_defaultImageWidthInput.State.IsValid && WidthInputs.All(i => i.State.IsValid);

            return new FormElementState<WidthsInputValue>
            {
                Value = new WidthsInputValue
                {
                    WidthComparisonMode = WidthComparisonMode,
                    DefaultWidth = canParseDefaultWidth ? defaultWidth : default(int?),
                    ScreenAndImageWidthsList = ScreenAndImageWidthsList
                },
                IsValid = isValid,
            };
        }
    }

    private WidthComparisonMode WidthComparisonMode
    {
        get;
        set
        {
            field = value;
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, ScreenAndImageWidthsList));
        }
    } = WidthComparisonMode.LTE;
  
    public IEnumerable<ScreenAndImageWidths> ScreenAndImageWidthsList
    {
        get
        {
            var sorted = field.Order();
            return WidthComparisonMode == WidthComparisonMode.GTE ? sorted : sorted.Reverse();
        }
        set
        {
            field = value;
           
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, ScreenAndImageWidthsList));
        }
    } = new List<ScreenAndImageWidths>();

    private Layout _widthInputsLayout;

    private TextInput _defaultImageWidthInput;
    private readonly int _minWidth = 1;
    private readonly int _maxWidth = 20_000;
    
    public WidthsInput()
    {
        InitializeComponent();
        InitializeChildComponents();
    }

    private void InitializeChildComponents()
    {
        var label = new Label()
        {
            Text = "Widths"
        };

        var modeInput = new RadioButtonGroup(
        [
            new RadioButtonGroupItem()
            {
                Content = "Max-Width",
                Value = WidthComparisonMode.LTE.ToString()
            },
            new RadioButtonGroupItem()
            {
                Content = "Min-Width",
                Value = WidthComparisonMode.GTE.ToString()
            },

        ], WidthComparisonMode.LTE.ToString(), "WidthComparisonModeGroup")
        {
            LabelText = "Select the media query to use for each screen width"
        };

        var screenWidthInputContainer = new HorizontalStackLayout();
        
        var screenWidthInput = new TextInput("",
            ValidateScreenWidthInputValue,
            FormElementHelpers.AllowOnlyDigits)
        {
            LabelText = "Add a screen width."
        };

        var addScreenWidthButton = new Button()
        {
            Text = "+"
        };

        addScreenWidthButton.Clicked += (sender, e) =>
        {
            if (screenWidthInput.State.IsValid && int.TryParse(screenWidthInput.Value, out var width))
            {
                ScreenAndImageWidthsList = ScreenAndImageWidthsList.Append(new ScreenAndImageWidths
                {
                    ScreenWidth = width
                });
                
                screenWidthInput.Reset();
            }
            else
            {
                screenWidthInput.RevealErrors();
            }
        };

        screenWidthInputContainer.Children.Add(screenWidthInput);
        screenWidthInputContainer.Children.Add(addScreenWidthButton);
        
        _widthInputsLayout = new VerticalStackLayout();
        
        var widthInputsHeader = new HorizontalStackLayout();
        
        widthInputsHeader.Children.Add(new Label
        {
            Text = "Screen Widths"
        });
        
        widthInputsHeader.Children.Add(new Label
        {
            Text = "Image Widths"
        });
        
        _widthInputsLayout.Children.Add(widthInputsHeader);
        
        var defaultImageWidthContainer = new HorizontalStackLayout();
        defaultImageWidthContainer.Children.Add(new Label
        {
            Text = "Default"
        });
        
        _defaultImageWidthInput = new WidthInput();
        defaultImageWidthContainer.Children.Add(_defaultImageWidthInput);
        _widthInputsLayout.Children.Add(defaultImageWidthContainer);
        
        BindableLayout.SetItemsSource(_widthInputsLayout, ScreenAndImageWidthsList);
        BindableLayout.SetItemTemplate(_widthInputsLayout, new DataTemplate(() =>
        {
            Console.WriteLine(this.BindingContext);
            return new Label()
            {
                Text = "Hello"
            };
        }));
        
        MainLayout.Children.Add(label);
        MainLayout.Children.Add(modeInput);
        MainLayout.Children.Add(screenWidthInputContainer);
        MainLayout.Children.Add(_widthInputsLayout);
    }
    
    public void RevealErrors()
    {
        throw new NotImplementedException();
    }

    private ValidatorFuncResult ValidateScreenWidthInputValue(string value)
    {
        var canParse = int.TryParse(value, out var width);

        if (canParse)
        {
            if (State.Value.ScreenAndImageWidthsList.All(w => w.ScreenWidth != width))
            {
                return new ValidatorFuncResult(
                    true,
                    ""
                );
            }

            return new ValidatorFuncResult(
                false,
                "Duplicate screen width."
            );
        }


        return new ValidatorFuncResult(
            false,
            "Please enter a screen width."
        );
    }
}