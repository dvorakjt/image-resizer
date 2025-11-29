namespace ImageResizer.DataModel.Formats;

public class FormatsFormGroupValue
{
    public IEnumerable<ImageFileFormat> SelectedFormats { get; init; }
    public AVIFOptionsFormGroupValue AVIFOptions { get; init; }
    public WebPOptionsFormGroupValue WebPOptions { get; init; }
    public JPEGOptionsFormGroupValue JPEGOptions { get; init; }
}