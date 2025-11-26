namespace ImageResizer.DataModel.Formats;

public record class FormatsFormGroupValue
{
    public IEnumerable<ImageFileFormats> SelectedFormats { get; init; }
    public AVIFOptionsFormGroupValue AVIFOptions { get; init; }
    public WebPOptionsFormGroupValue WebPOptions { get; init; }
    public JPEGOptionsFormGroupValue JPEGOptions { get; init; }
}