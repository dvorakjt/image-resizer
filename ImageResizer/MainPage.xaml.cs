using ImageResizer.FormControls;
using ImageResizer.DataModel;
using ImageResizer.FormGroups.Formats;
using ImageResizer.FormGroups.Output;

namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var formatsFormGroup = new FormatsFormGroup();
        MainLayout.Children.Add(formatsFormGroup);

        var outputFormGroup = new OutputFormGroup();
        MainLayout.Children.Add(outputFormGroup);
    }
}