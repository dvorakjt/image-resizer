using System.Collections.Specialized;
using System.ComponentModel;
using ImageResizer.Models;
using ImageResizer.ViewModels;

namespace ImageResizer.Views;

public class WidthInput : TextInput
{
    public WidthInput() : base("",
        FormElementHelpers.CreateMinMaxValidator(1, 20_000, "Please enter a valid width (min. 1, max. 20,000)."),
        FormElementHelpers.AllowOnlyDigits)
    {
        MaxLength = 5;
    }
}

public class ScreenAndImageWidths : IComparable<ScreenAndImageWidths>
{
    public required int ScreenWidth { get; init; }
    public int? ImageWidth { get; set; }
    
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

public partial class WidthsInput : ContentView, IFormElement<WidthsInputValue>, IFormElementWithErrorDisplay
{ 
    public event EventHandler<FormElementStateChangedEventArgs<WidthsInputValue>>? StateChanged;

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

    private ILiveSortedList<ScreenAndImageWidths> ScreenAndImageWidthsList = new LiveSortedList<ScreenAndImageWidths>()
        { IsReversed = true };
    
    private WidthComparisonMode WidthComparisonMode
    {
        get;
        set
        {
            field = value;
            ScreenAndImageWidthsList.IsReversed = field == WidthComparisonMode.LTE;
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
        }
    } = WidthComparisonMode.LTE;
    
    private Layout _widthInputsLayout;
    private TextInput _defaultImageWidthInput;
    
    public WidthsInput()
    {
        InitializeComponent();
        WatchScreenWidthsList();
        InitializeChildComponents();
    }

    // There is no need to watch the ListReset event because the state update is already triggered when the 
    // mode is changed.
    private void WatchScreenWidthsList()
    {
        ScreenAndImageWidthsList.ItemAdded += (sender, e) =>
        {
            Console.WriteLine("Adding screen width");
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
        };

        ScreenAndImageWidthsList.ItemRemoved += (sender, e) =>
        {
            StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
        };
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

        modeInput.StateChanged += (sender, e) =>
        {
            WidthComparisonMode = e.State.Value == WidthComparisonMode.LTE.ToString() ? WidthComparisonMode.LTE :  WidthComparisonMode.GTE;
        };

        var screenWidthInputContainer = new HorizontalStackLayout();
        
        var screenWidthInput = new WidthInput();

        var addScreenWidthButton = new Button()
        {
            Text = "+"
        };

        addScreenWidthButton.Clicked += (sender, e) =>
        {
            if (screenWidthInput.State.IsValid && int.TryParse(screenWidthInput.Value, out var width))
            {
                ScreenAndImageWidthsList.Add(new ScreenAndImageWidths { ScreenWidth = width });
                screenWidthInput.Reset();
            }
            else
            {
                screenWidthInput.RevealErrors();
            }
            
            Console.WriteLine(ScreenAndImageWidthsList.Count());
        };

        screenWidthInputContainer.Children.Add(screenWidthInput);
        screenWidthInputContainer.Children.Add(addScreenWidthButton);
        
     
        /* just creating this so there are no errors for now */        
        _defaultImageWidthInput = new WidthInput();

        _widthInputsLayout = new VerticalStackLayout();
        
        DynamicListFactory.MakeDynamic<ILiveSortedList<ScreenAndImageWidths>, ScreenAndImageWidths>(_widthInputsLayout, ScreenAndImageWidthsList, (screenAndImageWidth) =>
        {
            var row = new HorizontalStackLayout();
            var widthInputLabel = new Label()
            {
                Text = screenAndImageWidth.ScreenWidth.ToString()
            };
            
            row.Children.Add(widthInputLabel);
            
            var input = new WidthInput();
            input.StateChanged += (sender, e) =>
            {
                if (e.State.IsValid && int.TryParse(e.State.Value, out var width))
                {
                    screenAndImageWidth.ImageWidth = width;
                }
                else
                {
                    screenAndImageWidth.ImageWidth = null;
                }
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
            };
            
            row.Children.Add(input);
            
            var removeWidthButton = new Button()
            {
                Text = "-"
            };
            
            removeWidthButton.Clicked += (sender, e) =>
            {
                ScreenAndImageWidthsList.Remove(screenAndImageWidth);
                StateChanged?.Invoke(this, new FormElementStateChangedEventArgs<WidthsInputValue>(State));
            };
            
            row.Children.Add(removeWidthButton);

            return row;
        });
        
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