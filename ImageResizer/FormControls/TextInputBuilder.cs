using ImageResizer.DataModel;

namespace ImageResizer.FormControls;

public class TextInputBuilder
{
    private string? _labelText = null;
    private string _defaultValue = "";
    private Func<string, IValidatorResult> _validator = (string value) =>
        new ValidatorResult { IsValid = true, ErrorMessage = "" };
    private int _maxLength = int.MaxValue;
    private bool _numeric = false;
    private bool _allowZero = false;
    private int? _widthRequest = null;

    public TextInputBuilder WithLabel(string labelText)
    {
        _labelText = labelText;
        return this;
    }

    public TextInputBuilder WithDefaultValue(string defaultValue)
    {
        _defaultValue = defaultValue;
        return this;
    }

    public TextInputBuilder WithValidator(Func<string, IValidatorResult> validator)
    {
        _validator = validator;
        return this;
    }

    public TextInputBuilder WithMaxLength(int maxLength)
    {
        _maxLength = maxLength;
        return this;
    }

    public TextInputBuilder Numeric()
    {
        _numeric = true;
        return this;
    }

    public TextInputBuilder NumericAllowZero()
    {
        _numeric = true;
        _allowZero = true;
        return this;
    }

    public TextInputBuilder WithWidthRequest(int widthRequest)
    {
        _widthRequest = widthRequest;
        return this;
    }

    public TextInput Build()
    {
        var textInput = new TextInput(
            _labelText, _defaultValue, _validator, _maxLength, _numeric, _allowZero);

        if (_widthRequest != null)
        {
            textInput.WidthRequest = _widthRequest.Value;
        }
        
        return textInput;
    }
}