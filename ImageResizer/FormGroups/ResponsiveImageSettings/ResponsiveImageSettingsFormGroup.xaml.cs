using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageResizer.DataModel;
using ImageResizer.DataModel.ResponsiveImageSettings;
using ImageResizer.FormControls;
using Microsoft.Maui.Layouts;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class ResponsiveImageSettingsFormGroup : ContentView, IFormElement<ResponsiveImageSettingsFormGroupValue>
{
    public IFormElementState<ResponsiveImageSettingsFormGroupValue> State
    {
        get
        {
            ResponsiveImageStrategy strategy;
            bool isValid = _strategyRadioButtonGroup.State.IsValid;

            if (_strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.Densities.ToString())
            {
                strategy = ResponsiveImageStrategy.Densities;
            }
            else if (_strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.Widths.ToString())
            {
                strategy = ResponsiveImageStrategy.Widths;
            }
            else if (_strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.MediaQueries.ToString())
            {
                strategy = ResponsiveImageStrategy.MediaQueries;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported responsive image strategy:  {_strategyRadioButtonGroup.State.Value}");
            }

            switch (strategy)
            {
                case ResponsiveImageStrategy.Densities:
                    if(!_densitiesFormGroup.State.IsValid) isValid = false;
                    break;
                case ResponsiveImageStrategy.Widths:
                    if(!_widthsFormGroup.State.IsValid) isValid = false;
                    break;
                case ResponsiveImageStrategy.MediaQueries:
                    if(!_mediaQueriesFormGroup.State.IsValid) isValid = false;
                    break;
            }

            return new FormElementState<ResponsiveImageSettingsFormGroupValue>
            {
                Value = new ResponsiveImageSettingsFormGroupValue
                {
                    ResponsiveImageStrategy = strategy,
                    DensitiesStrategyOptions = _densitiesFormGroup.State.Value,
                    WidthsStrategyOptions = _widthsFormGroup.State.Value,
                    MediaQueriesStrategyOptions = _mediaQueriesFormGroup.State.Value,
                },
                IsValid = isValid,
                ErrorMessage = ""
            };
        }
    }

    public event EventHandler<IFormElementState<ResponsiveImageSettingsFormGroupValue>>? StateChanged;
    
    private CustomRadioButtonGroup _strategyRadioButtonGroup;
    private DensitiesFormGroup _densitiesFormGroup;
    private WidthsFormGroup _widthsFormGroup;
    private MediaQueriesFormGroup _mediaQueriesFormGroup;
    
    public ResponsiveImageSettingsFormGroup()
    {
        InitializeComponent();
        InitializeNestedFormGroups();
    }

    public void DisplayErrors()
    {
        _strategyRadioButtonGroup.DisplayErrors();
        _densitiesFormGroup.DisplayErrors();
        _widthsFormGroup.DisplayErrors();
        _mediaQueriesFormGroup.DisplayErrors();
    }

    public void Revalidate()
    {
        _strategyRadioButtonGroup.Revalidate();
        _densitiesFormGroup.Revalidate();
        _widthsFormGroup.Revalidate();
        _mediaQueriesFormGroup.Revalidate();
    }

    public void Reset()
    {
        _strategyRadioButtonGroup.Reset();
        _densitiesFormGroup.Reset();
        _widthsFormGroup.Reset();
        _mediaQueriesFormGroup.Reset();
    }

    private void InitializeNestedFormGroups()
    {
       InitializeStrategyRadioGroup();
       InitializeDensitiesFormGroup();
       InitializeWidthsFormGroup();
       InitializeMediaQueriesFormGroup();
    }

    private void InitializeStrategyRadioGroup()
    {
        _strategyRadioButtonGroup = new CustomRadioButtonGroup
        (
            [
                new CustomRadioButtonGroupItem
                {
                    Value = ResponsiveImageStrategy.Densities.ToString(),
                    Content = "Densities",
                },
                new CustomRadioButtonGroupItem
                {
                    Value = ResponsiveImageStrategy.Widths.ToString(),
                    Content = "Widths",
                },
                new CustomRadioButtonGroupItem
                {
                    Value = ResponsiveImageStrategy.MediaQueries.ToString(),
                    Content = "Media Queries",
                }
            ],
            ResponsiveImageStrategy.Densities.ToString(),
            "ResponsiveImageStrategy"
        )
        {
            JustifyContent = FlexJustify.SpaceBetween
        };
        
        _strategyRadioButtonGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        Header.Children.Add(_strategyRadioButtonGroup);
    }

    private void InitializeDensitiesFormGroup()
    {
        _densitiesFormGroup = new DensitiesFormGroup()
        {
            IsVisible = _strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.Densities.ToString(),
        };
        
        _densitiesFormGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        _strategyRadioButtonGroup.StateChanged += (sender, e) =>
        {
            _densitiesFormGroup.IsVisible = e.Value == ResponsiveImageStrategy.Densities.ToString();
        };

        RootLayout.Children.Add(_densitiesFormGroup);
    }

    private void InitializeWidthsFormGroup()
    {
        _widthsFormGroup = new WidthsFormGroup()
        {
            IsVisible = _strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.Widths.ToString(),
        };
        
        _widthsFormGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        _strategyRadioButtonGroup.StateChanged += (sender, e) =>
        {
            _widthsFormGroup.IsVisible = e.Value == ResponsiveImageStrategy.Widths.ToString();
        };

        RootLayout.Children.Add(_widthsFormGroup);
    }

    private void InitializeMediaQueriesFormGroup()
    {
        _mediaQueriesFormGroup = new MediaQueriesFormGroup()
        {
            IsVisible = _strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.MediaQueries.ToString(),
        };
        
        _mediaQueriesFormGroup.StateChanged += (sender, e) => StateChanged?.Invoke(this, State);
        _strategyRadioButtonGroup.StateChanged += (sender, e) =>
        {
            _mediaQueriesFormGroup.IsVisible = e.Value == ResponsiveImageStrategy.MediaQueries.ToString();
        };

        RootLayout.Children.Add(_mediaQueriesFormGroup);
    }
}