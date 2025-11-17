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
                Width = AppDimensions.DEFAULT_WIDTH,
                Height = AppDimensions.DEFAULT_HEIGHT,
                MinimumWidth = AppDimensions.MIN_WIDTH,
                MinimumHeight = AppDimensions.MIN_HEIGHT,
            };
        }

        private void InitializeHandlers()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveNativeFocusStyles", (handler, view) =>
            {
#if MACCATALYST
                // Remove the border that appears around in-focus Entry elements on Mac
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });
        }
    }
}