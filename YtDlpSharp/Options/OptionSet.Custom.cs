namespace YtDlpSharp.Options;

public partial class OptionSet
{
    public IOption[] CustomOptions { get; set; } = [];

    public void AddCustomOption<T>(string optionString, T value)
    {
        var option = new Option<T>(true, optionString) { Value = value };
        CustomOptions = [.. CustomOptions, option];
    }

    public void SetCustomOption<T>(string optionString, T value)
    {
        foreach (var iOption in CustomOptions.Where(o => o.OptionStrings.Contains(optionString)))
        {
            if (iOption is Option<T> option)
                option.Value = value;
            else
                throw new ArgumentException($"Value passed to option '{optionString}' has invalid type '{value?.GetType()}'.");
        }
    }

    public void DeleteCustomOption(string optionString)
    {
        CustomOptions = CustomOptions.Where(o => !o.OptionStrings.Contains(optionString)).ToArray();
    }
}
