using Microsoft.Extensions.Logging;

namespace ImageResizer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("IBMPlexSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("IBMPlexSans-SemiBold.ttf", "IBMPlexSansSemiBold");
                fonts.AddFont("IBMPlexSans-Bold.ttf", "IBMPlexSansBold");
            });
                

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
