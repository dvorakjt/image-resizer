namespace ImageResizer.ViewFactories;

public static class SectionLayoutFactory
{
    public static Layout CreateSectionLayout()
    {
        var sectionLayout = new VerticalStackLayout();
        sectionLayout.MinimumWidthRequest = AppDimensions.CONTENT_WIDTH;
        sectionLayout.MaximumWidthRequest = AppDimensions.CONTENT_WIDTH;
        return sectionLayout;
    }
}