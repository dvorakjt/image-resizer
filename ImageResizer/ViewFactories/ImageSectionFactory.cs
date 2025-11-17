using ImageResizer.ViewModels;
using ImageResizer.Views;

namespace ImageResizer.ViewFactories;

public class ImageSection
{
    public Layout SectionLayout { get; init; }
    public ImagePicker  ImagePicker { get; init; }
    public TextInput  AltTextInput { get; init; }
}

public static class ImageSectionFactory
{
    public static ImageSection Create()
    {
        var sectionLayout = SectionLayoutFactory.CreateSectionLayout();
        var imagePicker = new ImagePicker()
        {
            Margin = new Thickness(0, 0, 0, 10)
        };
        
        sectionLayout.Children.Add(imagePicker);

        var altTextInput = new TextInput(
            "",
            FormElementHelpers.CreateRequiredFieldValidator
            (
                "Please enter alt text for the image"
            )
        )
        {
            LabelText = "Alt Text",
            MaxLength = 255
        };
        
        sectionLayout.Children.Add(altTextInput);

        return new ImageSection()
        {
            SectionLayout = sectionLayout,
            ImagePicker = imagePicker,
            AltTextInput = altTextInput
        };
    }
}