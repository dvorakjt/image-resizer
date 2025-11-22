

namespace ImageResizer;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage())
        {
            MinimumWidth = 476,
            Width = 476,
            Height = 768
        };
    }
}