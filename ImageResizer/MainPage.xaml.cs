using ImageResizer.FormControls;
using ImageResizer.DataModel;
namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var textInput = new TextInputBuilder().Build();
        MainLayout.Children.Add(textInput);
        
        var cb1 = new IRCheckbox()
        {
            IsChecked = true,
            LabelText = "Starts Checked and Enabled"
        };
        
        var cb2 = new IRCheckbox()
        {
            IsChecked = false,
            LabelText = "Starts Unchecked and Enabled"
        };

        var cb3 = new IRCheckbox()
        {
            IsEnabled = false,
            LabelText = "Starts Unchecked and Disabled"
        };

        var cb4 = new IRCheckbox()
        {
            IsChecked = true,
            IsEnabled = false,
            LabelText = "Starts Checked and Disabled"
        };
        
        MainLayout.Children.Add(cb1);
        MainLayout.Children.Add(cb2);
        MainLayout.Children.Add(cb3);
        MainLayout.Children.Add(cb4);
    }
}