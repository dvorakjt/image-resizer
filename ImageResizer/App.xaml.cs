using Microsoft.Extensions.DependencyInjection;

namespace ImageResizer
{
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
                Width = 540,
                Height = 768,
                MinimumWidth = 412,
                MinimumHeight = 540
            };
        }
    }
}