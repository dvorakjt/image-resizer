using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageResizer.DataModel;
using ImageResizer.FormControls;
using Microsoft.Maui.Layouts;

namespace ImageResizer.FormGroups.ResponsiveImageSettings;

public partial class ResponsiveImageSettingsFormGroup : ContentView
{
    CustomRadioButtonGroup _strategyRadioButtonGroup;
    DensitiesFormGroup _densitiesFormGroup;
    WidthsFormGroup _widthsFormGroup;
    MediaQueriesFormGroup _mediaQueriesFormGroup;
    
    public ResponsiveImageSettingsFormGroup()
    {
        InitializeComponent();
        InitializeNestedFormGroups();
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
        
        Header.Children.Add(_strategyRadioButtonGroup);
    }

    private void InitializeDensitiesFormGroup()
    {
        _densitiesFormGroup = new DensitiesFormGroup()
        {
            IsVisible = _strategyRadioButtonGroup.State.Value == ResponsiveImageStrategy.Densities.ToString(),
        };
        
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
        
        _strategyRadioButtonGroup.StateChanged += (sender, e) =>
        {
            _mediaQueriesFormGroup.IsVisible = e.Value == ResponsiveImageStrategy.MediaQueries.ToString();
        };

        RootLayout.Children.Add(_mediaQueriesFormGroup);
    }
}