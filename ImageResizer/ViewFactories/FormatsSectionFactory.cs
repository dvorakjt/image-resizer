using ImageResizer.Models;
using ImageResizer.Views;
using ImageResizer.ViewModels;

namespace ImageResizer.ViewFactories;

public class FormatsSection
{
    public Layout SectionLayout { get; init; }
    public CheckboxGroup SelectedFormats { get; init;  }
    public QualityAndEffortInput AVIFOptionsInput { get; init; }
    public QualityAndEffortInput WebPOptionsInput { get; init; }
    public TextInput JPGQualityInput { get; init; }
}

public static class FormatsSectionFactory
{
    public static FormatsSection Create()
    {
        var sectionLayout = SectionLayoutFactory.CreateSectionLayout();
        var heading = CreateHeading();
        sectionLayout.Children.Add(heading);

        var selectedFormats = new CheckboxGroup
        (
            [
                new CheckboxGroupItem()
                {
                    Label = OutputFormat.AVIF.ToString(),
                    Value = OutputFormat.AVIF.ToFileExtension(),
                    IsChecked = true,
                },
                new CheckboxGroupItem()
                {
                    Label = OutputFormat.WebP.ToString(),
                    Value = OutputFormat.WebP.ToFileExtension(),
                    IsChecked = true,
                },
                new CheckboxGroupItem()
                {
                    Label = OutputFormat.JPG.ToString(),
                    Value = OutputFormat.JPG.ToFileExtension(),
                    IsChecked = true,
                    IsFrozen = true
                },
            ]
        )
        {
            LabelText = "Selected Output Formats",
            Margin = new Thickness(0, 0, 0, 10),
        };
        
        sectionLayout.Children.Add(selectedFormats);

        var avifFormatOptions = CreateAVIFOptionsInput(selectedFormats);
        sectionLayout.Children.Add(avifFormatOptions);

        var webPFormatOptions = CreateWebPOptionsInput(selectedFormats);
        sectionLayout.Children.Add(webPFormatOptions);

        var jpgQualityInput = CreateJPGQualityInput(selectedFormats);
        var jpgOptions = CreateJPGOptionsSection(jpgQualityInput);
        sectionLayout.Children.Add(jpgOptions);

        return new FormatsSection()
        {
            SectionLayout = sectionLayout,
            SelectedFormats = selectedFormats,
            AVIFOptionsInput = avifFormatOptions,
            WebPOptionsInput = webPFormatOptions,
            JPGQualityInput = jpgQualityInput
        };
    }

    private static Label CreateHeading()
    {
        var heading = new Label()
        {
            Text = "Formats",
            StyleClass = ["SubHeading1"],
        };
        return heading;
    }

    private static QualityAndEffortInput CreateAVIFOptionsInput(CheckboxGroup selectedFormats)
    {
        var avifFormatOptions = new QualityAndEffortInput
        (
            (0, 100),
            (0, 9),
            50,
            4
        )
        {
            LabelText = "AVIF Options",
            Margin = new Thickness(0, 0, 0, 10),
            IsVisible = selectedFormats.State.Value.Contains(OutputFormat.AVIF.ToFileExtension()),
        };
        
        selectedFormats.StateChanged += (sender, e) => 
            avifFormatOptions.IsVisible = selectedFormats.State.Value.Contains(OutputFormat.AVIF.ToFileExtension());
        
        return avifFormatOptions;
    }
    
    private static QualityAndEffortInput CreateWebPOptionsInput(CheckboxGroup selectedFormats)
    {
        var webPFormatOptions = new QualityAndEffortInput
        (
            (0, 100),
            (0, 6),
            75,
            4
        )
        {
            LabelText = "WebP Options",
            Margin = new Thickness(0, 0, 0, 10),
            IsVisible = selectedFormats.State.Value.Contains(OutputFormat.WebP.ToFileExtension()),
        };
        
        selectedFormats.StateChanged += (sender, e) => 
            webPFormatOptions.IsVisible = selectedFormats.State.Value.Contains(OutputFormat.WebP.ToFileExtension());

        return webPFormatOptions;
    }

    private static TextInput CreateJPGQualityInput(CheckboxGroup selectedFormats)
    {
        var jpgQualityInput = new TextInput
        (
            90.ToString(),
            FormElementHelpers.CreateMinMaxValidator(0, 100, "Please enter a number between 0 and 100."),
            FormElementHelpers.AllowOnlyDigits
        )
        {
            LabelText = "Quality",
            MaxLength = 3
        };
        
        return jpgQualityInput;
    }

    private static Layout CreateJPGOptionsSection(TextInput jpgOptionsInput)
    {
        var section = new VerticalStackLayout();

        var heading = new Label()
        {
            Text = "JPG Options",
            StyleClass = ["SubHeading2"],
        };
        
        section.Children.Add(heading);
        section.Children.Add(jpgOptionsInput);
        return section;
    }
}