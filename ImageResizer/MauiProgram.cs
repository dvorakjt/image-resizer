using Microsoft.Extensions.Logging;
using ImageResizer.FormControls;

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
                fonts.AddFont("IBMPlexSans-Regular.ttf", "OpenSansRegular");
                ;
                fonts.AddFont("IBMPlexSans-Bold.ttf", "IBMPlexSansBold");
            });

#if MACCATALYST
        // Remove the border that appears around in-focus Entry elements on Mac
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveNativeFocusStyles",
            (handler, view) => { handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None; });

        // Prevent input of non-digits for numeric Entries
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CancelInvalidInput", (handler, view) =>
        {
            if (handler.PlatformView is UIKit.UITextField nativeTextField)
            {
                nativeTextField.ShouldChangeCharacters += (textField, range, replacementString) =>
                {
                    var maybeCustomTextBox = view.Parent?.Parent?.Parent;
                    if (
                        maybeCustomTextBox is TextInput customTextInput &&
                        (
                            customTextInput.Accepts == AcceptedCharacters.WholeNumbers ||
                            customTextInput.Accepts == AcceptedCharacters.PositiveIntegers
                        )
                    )
                    {
                        var oldText = textField.Text ?? string.Empty;
                        var newText = oldText.Substring(0, (int)range.Location)
                                      + replacementString
                                      + oldText.Substring((int)(range.Location + range.Length));

                        return FormControlHelpers.IsIntegerOrEmptyString (
                            newText,
                            customTextInput.Accepts == AcceptedCharacters.WholeNumbers
                        );
                    }
                   
                    return true;
                };
            }
        });
#endif

#if WINDOWS
        // Remove default padding and minimum size requirements from Entry control
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemovePaddingAndMargin", (handler, view) => 
        {
            if(handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox nativeTextBox)
            {
              nativeTextBox.Padding = new Microsoft.UI.Xaml.Thickness(0);
              nativeTextBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
              nativeTextBox.MinHeight = 0;
            }

            // throw new Exception(view.Parent?.Parent?.Parent?.ToString());
        });

        // Prevent input of non-digits for numeric Entries
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CancelInvalidInput", (handler, view) => 
        {
            if(handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox nativeTextBox)
            {
                nativeTextBox.BeforeTextChanging += (sender, e) =>
                {
                    var maybeCustomTextInput = view.Parent?.Parent?.Parent;
                    if(
                        maybeCustomTextInput is TextInput customTextInput &&
                        (
                            customTextInput.Accepts == AcceptedCharacters.WholeNumbers ||
                            customTextInput.Accepts == AcceptedCharacters.PositiveIntegers
                        )
                    )
                    {
                        if(!FormControlHelpers.IsIntegerOrEmptyString(e.NewText, customTextInput.Accepts == AcceptedCharacters.WholeNumbers))
                        {
                            e.Cancel = true;
                        }
                    }
                };
            }
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}