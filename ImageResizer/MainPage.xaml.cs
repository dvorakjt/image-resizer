using ImageResizer.FormControls;
using ImageResizer.DataModel;
namespace ImageResizer;

public partial class MainPage : ContentPage
{ 
    public MainPage()
    {
        InitializeComponent();
        var radioButton1 = new IRRadioButton()
        {
            LabelText = "Apples",
            IsChecked = true,
            GroupName = "Fruits"
        };

        var radioButton2 = new IRRadioButton()
        {
            LabelText = "Oranges",
            IsChecked = false,
            GroupName = "Fruits"
        };
        
        MainLayout.Children.Add(radioButton1);
        MainLayout.Children.Add(radioButton2);
    }
}