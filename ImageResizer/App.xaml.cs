namespace ImageResizer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeHandlers();
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

        private void InitializeHandlers()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveNativeFocusStyles", (handler, view) =>
            {
#if MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif

#if WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness()
                {
                    Bottom = 0,
                    Top = 0,
                    Left = 0,
                    Right = 0,
                };
#endif
            });
        }
    }
}