using ImageResizer.FormControls;
using ImageResizer.DataModel;
using ImageResizer.FormGroups.Formats;

namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var formatsFormGroup = new FormatsFormGroup();
        MyBorder.Content = formatsFormGroup;
    }
}