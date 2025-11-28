using ImageResizer.DataModel.Formats;
using ImageResizer.DataModel.Output;
using ImageResizer.DataModel.ResponsiveImageSettings;
using ImageResizer.DataModel.TheImage;

namespace ImageResizer.ImageProcessing;

public static class ImageProcessor
{
    private static IImageWriter _imageWriter = new NetVipsImageWriter();
    
    public static async Task<string> ProcessImage(        
        TheImageFormGroupValue theImageFormGroupValue,
        ResponsiveImageSettingsFormGroupValue responsiveImageSettingsFormGroupValue,
        FormatsFormGroupValue formatsFormGroupValue,
        OutputFormGroupValue outputFormGroupValue
    )
    {
        await ResizeReformatAndSaveImages(
            theImageFormGroupValue, 
            responsiveImageSettingsFormGroupValue, 
            formatsFormGroupValue, 
            outputFormGroupValue
        );

        // this will ultimately become the tag which will be copied to the clipboard
        return "";
    }

    private static async Task ResizeReformatAndSaveImages(
        TheImageFormGroupValue theImageFormGroupValue,
        ResponsiveImageSettingsFormGroupValue responsiveImageSettingsFormGroupValue,
        FormatsFormGroupValue formatsFormGroupValue,
        OutputFormGroupValue outputFormGroupValue
    )
    {
        if (theImageFormGroupValue.ImageStream == null)
        {
            throw new ArgumentNullException(nameof(theImageFormGroupValue.ImageStream));
        }
        
        
        /*
            Ensure that the stream position is set to 0 before attempting to copy it.
            This should be redundant, as the stream will be reset after updating the 
            thumbnail and after resizing it.
        */
        theImageFormGroupValue.ImageStream.Position = 0;
        
        // Convert the image stream to a byte array
        using var memoryStream = new MemoryStream();
        await theImageFormGroupValue.ImageStream.CopyToAsync(memoryStream);
        var imageData = memoryStream.ToArray();

        // Reset the position of the stream, which will be at the end after copying it.
        theImageFormGroupValue.ImageStream.Position = 0;

        var outputPath = new ImagePath(
            outputFormGroupValue.PathToPublicDirectory,
            outputFormGroupValue.PathFromPublicDirectory,
            outputFormGroupValue.Filename,
            outputFormGroupValue.VersionId
        );

        HashSet<int> widths = ReadWidths(responsiveImageSettingsFormGroupValue);
        
        ValidateFormatOptions(formatsFormGroupValue);

        await Task.WhenAll(formatsFormGroupValue.SelectedFormats.Select(format =>
        {
            switch (format)
            {
                case ImageFileFormats.AVIF:
                    return _imageWriter.ResizeReformatAndSaveAsAVIF(
                        imageData,
                        widths,
                        outputPath,
                        formatsFormGroupValue.AVIFOptions.Quality!.Value,
                        formatsFormGroupValue.AVIFOptions.Effort!.Value
                    );
                case ImageFileFormats.WebP:
                    return _imageWriter.ResizeReformatAndSaveAsWebP(
                        imageData,
                        widths,
                        outputPath,
                        formatsFormGroupValue.WebPOptions.Quality!.Value,
                        formatsFormGroupValue.WebPOptions.Effort!.Value
                    );
                case ImageFileFormats.JPEG:
                    return _imageWriter.ResizeReformatAndSaveAsJPEG(
                        imageData,
                        widths,
                        outputPath,
                        formatsFormGroupValue.JPEGOptions.Quality!.Value
                    );
                default:
                    return Task.CompletedTask;
            }
        }));
    }

    private static HashSet<int> ReadWidths(ResponsiveImageSettingsFormGroupValue responsiveImageSettingsFormGroupValue)
    {
        switch (responsiveImageSettingsFormGroupValue.ResponsiveImageStrategy)
        {
            case ResponsiveImageStrategy.Densities:
                return ImageWidthsReader.GetImageWidths(responsiveImageSettingsFormGroupValue.DensitiesStrategyOptions);
            case ResponsiveImageStrategy.Widths:
                return ImageWidthsReader.GetImageWidths(responsiveImageSettingsFormGroupValue.WidthsStrategyOptions);
            case ResponsiveImageStrategy.MediaQueries:
                return ImageWidthsReader.GetImageWidths(responsiveImageSettingsFormGroupValue.MediaQueriesStrategyOptions);
            default:
                throw new ArgumentOutOfRangeException(nameof(responsiveImageSettingsFormGroupValue.ResponsiveImageStrategy));
        }
    }

    private static void ValidateFormatOptions(FormatsFormGroupValue formatsFormGroupValue)
    {
        if (formatsFormGroupValue.SelectedFormats.Contains(ImageFileFormats.AVIF))
        {
            if (formatsFormGroupValue.AVIFOptions.Quality == null)
            {
                throw new ArgumentNullException(nameof(formatsFormGroupValue.AVIFOptions.Quality));
            }
            
            if (formatsFormGroupValue.AVIFOptions.Effort == null)
            {
                throw new ArgumentNullException(nameof(formatsFormGroupValue.AVIFOptions.Effort));
            }
        }
        
        if(formatsFormGroupValue.SelectedFormats.Contains(ImageFileFormats.WebP))
        {
            if (formatsFormGroupValue.WebPOptions.Quality == null)
            {
                throw new ArgumentNullException(nameof(formatsFormGroupValue.WebPOptions.Quality));
            }
            
            if (formatsFormGroupValue.WebPOptions.Effort == null)
            {
                throw new ArgumentNullException(nameof(formatsFormGroupValue.WebPOptions.Effort));
            }
        }
        
        if(formatsFormGroupValue.SelectedFormats.Contains(ImageFileFormats.JPEG))
        {
            if (formatsFormGroupValue.JPEGOptions.Quality == null)
            {
                throw new ArgumentNullException(nameof(formatsFormGroupValue.JPEGOptions.Quality));
            }
        }
    }
}