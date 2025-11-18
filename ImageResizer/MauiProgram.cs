using Microsoft.Extensions.Logging;

namespace ImageResizer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("IBMPlexSans-Regular.ttf", "OpenSansRegular"); ;
                fonts.AddFont("IBMPlexSans-Bold.ttf", "IBMPlexSansBold");
            });
        
#if MACCATALYST
        // Remove the border that appears around in-focus Entry elements on Mac
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveNativeFocusStyles", (handler, view) =>
        {
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}