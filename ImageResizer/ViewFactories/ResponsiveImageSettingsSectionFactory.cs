using ImageResizer.Views;
using ImageResizer.Models;
using ImageResizer.ViewModels;

namespace ImageResizer.ViewFactories;

public class ResponsiveImageSettingsSection
{
    public Layout SectionLayout { get; init; }
    public IRRadioButtonGroup ResponsivenessModeInput { get; init; }
    public DensitiesInput DensitiesInput { get; init; }
    public WidthsInput WidthsInput { get; init; }
    public MediaQueriesInput MediaQueriesInput { get; init; }
}

public static class ResponsiveImageSettingsSectionFactory
{
    public static ResponsiveImageSettingsSection Create()
    {
        var sectionLayout = SectionLayoutFactory.CreateSectionLayout();
        var heading = CreateHeading();
        sectionLayout.Add(heading);
        
        var responsivenessModeInput = CreateResponsivenessModeInput();
        sectionLayout.Add(responsivenessModeInput);

        var densitiesInput = CreateDensitiesInput(responsivenessModeInput);
        sectionLayout.Add(densitiesInput);

        var widthsInput = CreateWidthsInput(responsivenessModeInput);
        sectionLayout.Add(widthsInput);
        
        var mediaQueriesInput = CreateMediaQueriesInput(responsivenessModeInput);
        sectionLayout.Add(mediaQueriesInput);

        return new ResponsiveImageSettingsSection()
        {
            SectionLayout = sectionLayout,
            ResponsivenessModeInput = responsivenessModeInput,
            DensitiesInput = densitiesInput,
            WidthsInput = widthsInput,
            MediaQueriesInput = mediaQueriesInput
        };
    }

    private static Label CreateHeading()
    {
        var heading = new Label()
        {
            Text = "Responsive Image Settings",
            StyleClass = ["SubHeading1"]
        };
        
        return heading;
    }

    private static IRRadioButtonGroup CreateResponsivenessModeInput()
    {
        var responsivenessModesInput = new IRRadioButtonGroup
        (
            [
                new RadioButtonGroupItem()
                {
                    Content = "Densities",
                    Value = ResponsivenessMode.Densities.ToString()
                },
                new RadioButtonGroupItem()
                {
                    Content = "Widths",
                    Value = ResponsivenessMode.Widths.ToString()
                },
                new RadioButtonGroupItem()
                {
                    Content = "Media Queries",
                    Value = ResponsivenessMode.MediaQueries.ToString()
                }
            ],
            ResponsivenessMode.Densities.ToString(),
            "ResponsivenessModes"
        )
        {
            LabelText = "Responsiveness Mode",
            Margin = new Thickness(0, 0, 0, 10)
        };
        
        return responsivenessModesInput;
    }

    private static DensitiesInput CreateDensitiesInput(IRRadioButtonGroup responsivenessModeInput)
    {
        var densitiesInput = new DensitiesInput()
        {
            IsVisible = responsivenessModeInput.State.Value == ResponsivenessMode.Densities.ToString()
        };

        responsivenessModeInput.StateChanged += (sender, e) =>
        {
            densitiesInput.IsVisible = e.State.Value == ResponsivenessMode.Densities.ToString();
        };
        
        return densitiesInput;
    }
    
    private static WidthsInput CreateWidthsInput(IRRadioButtonGroup responsivenessModeInput)
    {
        var widthsInput = new WidthsInput()
        {
            IsVisible = responsivenessModeInput.State.Value == ResponsivenessMode.Widths.ToString()
        };

        responsivenessModeInput.StateChanged += (sender, e) =>
        {
            widthsInput.IsVisible = e.State.Value == ResponsivenessMode.Widths.ToString();
        };
        
        return widthsInput;
    }

    private static MediaQueriesInput CreateMediaQueriesInput(IRRadioButtonGroup responsivenessModeInput)
    {
        var mediaQueriesInput = new MediaQueriesInput()
        {
            IsVisible = responsivenessModeInput.State.Value == ResponsivenessMode.MediaQueries.ToString()
        };

        responsivenessModeInput.StateChanged += (sender, e) =>
        {
            mediaQueriesInput.IsVisible = e.State.Value == ResponsivenessMode.MediaQueries.ToString();
        };
        
        return mediaQueriesInput;
    }
}